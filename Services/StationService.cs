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

        public async Task<IEnumerable<StationResponseDTO>> GetNearbyStationsAsync(decimal latitude, decimal longitude, double radius)
        {
            if (latitude == null || latitude < -90 || latitude > 90 || longitude == null || longitude < -180 || longitude > 180)
            {
                throw new ArgumentException("Invalid latitude or longitude parameters.");
            }

            double radiusInDegrees = radius / 111139.0;

            var stations = await _context.Stations
                .Where(s => !s.IsSoftDeleted && s.IsApproved && s.StationLatitude >= latitude - (decimal)radiusInDegrees
                            && s.StationLatitude <= latitude + (decimal)radiusInDegrees
                            && s.StationLongitude >= longitude - (decimal)radiusInDegrees
                            && s.StationLongitude <= longitude + (decimal)radiusInDegrees)
                .ToListAsync();


            var nearbyStations = stations
                .Where(s => CalculateDistance(latitude, longitude, s.StationLatitude, s.StationLongitude) <= radius)
                .Select(s => s.ToDto())
                .ToList();


            if (!nearbyStations.Any())
            {
                var osmStations = await FetchStationsFromOsmAsync(latitude, longitude, radius);
                foreach (var osmStation in osmStations)
                {
                    var station = osmStation.ToModel();
                    _context.Stations.Add(station);
                }
                await _context.SaveChangesAsync();
                nearbyStations = await _context.Stations
                    .Where(s => !s.IsSoftDeleted && s.IsApproved
                                && s.StationLatitude >= latitude - (decimal)radiusInDegrees
                                && s.StationLatitude <= latitude + (decimal)radiusInDegrees
                                && s.StationLongitude >= longitude - (decimal)radiusInDegrees
                                && s.StationLongitude <= longitude + (decimal)radiusInDegrees)
                    .Where(s => CalculateDistance(latitude, longitude, s.StationLatitude, s.StationLongitude) <= radius)
                    .Select(s => s.ToDto())
                    .ToListAsync();
            }

            return nearbyStations;
        }

        private double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            double R = 6371000; // Earth radius in meters
            double lat1Rad = (double)lat1 * Math.PI / 180;
            double lat2Rad = (double)lat2 * Math.PI / 180;
            double deltaLat = (double)(lat2 - lat1) * Math.PI / 180;
            double deltaLon = (double)(lon2 - lon1) * Math.PI / 180;

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance in meters
        }
        private async Task<List<StationResponseDTO>> FetchStationsFromOsmAsync(decimal latitude, decimal longitude, double radius)
        {
            var query = $"[out:json];node[amenity=fuel](around:{radius},{latitude},{longitude});out;";
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

            var station = request.ToModel(user.Username); 
            _context.Stations.Add(station);
            user.Points += 50; 
            await _context.SaveChangesAsync();
            return station.ToDto();
        }

        
    }
}
