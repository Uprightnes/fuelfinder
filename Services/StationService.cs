using System.Net.Http;
using System.Text.Json;
using FuelFinderApi.Configurations;
using FuelFinderApi.Data;
using FuelFinderApi.DTOs;
using FuelFinderApi.Mappers;
using FuelFinderApi.Models;
using FuelFinderApi.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FuelFinderApi.Services
{
    public class StationService : IStationService
    {
        private readonly ApplicationDBContext _context;
        private readonly HttpClient _httpClient;
        private readonly OverpassApiSettings _overpassSettings;

        public StationService(ApplicationDBContext context, IHttpClientFactory httpClientFactory, IOptions<OverpassApiSettings> overpassSettings)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
            _overpassSettings = overpassSettings.Value;
        }

        public async Task<IEnumerable<StationResponseDTO>> GetNearbyStationsAsync(decimal latitude, decimal longitude)
        {
            if (latitude == null || latitude < -90 || latitude > 90 || longitude == null || longitude < -180 || longitude > 180)
            {
                throw new ArgumentException("Invalid latitude or longitude parameters.");
            }

            var stations = await _context.Stations
                .Where(s => !s.IsSoftDeleted && s.IsApproved)
                .ToListAsync();

            if (!stations.Any())
            {
                var osmStations = await FetchStationsFromOsmAsync(latitude, longitude);
                foreach (var osmStation in osmStations)
                {
                    var station = osmStation.ToModel(); // Use manual mapper
                    _context.Stations.Add(station);
                }
                await _context.SaveChangesAsync();
                stations = await _context.Stations
                    .Where(s => !s.IsSoftDeleted && s.IsApproved)
                    .ToListAsync();
            }

            return stations.Select(s => s.ToDto()); // Use manual mapper
        }

        private async Task<List<StationResponseDTO>> FetchStationsFromOsmAsync(decimal latitude, decimal longitude)
        {
            var query = $"[out:json];node[amenity=fuel](around:10000,{latitude},{longitude});out;";
            var url = $"{_overpassSettings.BaseUrl}?data={Uri.EscapeDataString(query)}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var elements = doc.RootElement.GetProperty("elements").EnumerateArray();

            var stations = new List<StationResponseDTO>();
            foreach (var element in elements)
            {
                var tags = element.GetProperty("tags");
                stations.Add(new StationResponseDTO
                {
                    PlaceId = element.GetProperty("id").GetInt64().ToString(),
                    StationName = tags.TryGetProperty("name", out var name) ? name.GetString() : "Unknown",
                    StationLatitude = (decimal)element.GetProperty("lat").GetDouble(),
                    StationLongitude = (decimal)element.GetProperty("lon").GetDouble(),
                    StationAddress = tags.TryGetProperty("addr:full", out var addr) ? addr.GetString() : null
                });
            }

            return stations;
        }
        

        public async Task<StationResponseDTO> AddStationAsync(StationResponseDTO request, Guid userId)
        {
            var user = await _context.FuelFinderUsers.FirstOrDefaultAsync(u => u.FuelFinderUserId == userId && !u.IsSoftDeleted);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var station = request.ToModel(user.Username); // Use username as CreatedBy
            _context.Stations.Add(station);
            user.Points += 50; // Reward for submission
            await _context.SaveChangesAsync();
            return station.ToDto();
        }

        
    }
}
