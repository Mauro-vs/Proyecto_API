using Proyecto_API.Models;
using Proyecto_API.Config;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Proyecto_API.Services
{
    public class pilotoServices
    {
        private readonly HttpClient _client = new HttpClient();

        // Cache q almacena los perfiles
        private static Dictionary<string, pilotosModels> _cache = new Dictionary<string, pilotosModels>();

        public pilotoServices()
        {
            // si ya tiene la cabecera, no la añade (evita duplicados)
            if (!_client.DefaultRequestHeaders.Contains("x-api-key"))
            {
                _client.DefaultRequestHeaders.Add("x-api-key", ApiConfig.ApiKey);
                _client.DefaultRequestHeaders.Add("accept", "application/json");
            }
        }

        public async Task<pilotosModels> GetSeasonCompetitorsAsync(string idPiloto)
        {
            // si hay cache devuelve el perfil sin hacer la petición HTTP
            if (_cache.ContainsKey(idPiloto)) return _cache[idPiloto];

            try
            {
                string url = $"{ApiConfig.BaseUrl}/competitors/{idPiloto}/profile.json";
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode) // Manejo HTTP
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var piloto = JsonSerializer.Deserialize<pilotosModels>(json, options);

                    // Guardar en caché antes de devolver
                    if (piloto != null) _cache[idPiloto] = piloto;

                    return piloto;
                }
            }
            catch { }
            return null;
        }

        public async Task<List<pilotosInfo>> GetAllSeasonPilotosAsync()
        {
            try
            {
                string url = $"{ApiConfig.BaseUrl}/seasons/{ApiConfig.SeasonId}/competitors.json";
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var data = JsonSerializer.Deserialize<SeasonCompetitorsResponse>(json, options);
                    return data?.SeasonCompetitors;
                }

                // Plan B de IDs si la API Trial falla (para que siempre veas algo)
                return new List<pilotosInfo>
                {
                    new pilotosInfo { Id = "sr:competitor:21999" },
                    new pilotosInfo { Id = "sr:competitor:21997" },
                    new pilotosInfo { Id = "sr:competitor:8003" },
                    new pilotosInfo { Id = "sr:competitor:34971" },
                    new pilotosInfo { Id = "sr:competitor:172900" },
                    new pilotosInfo { Id = "sr:competitor:184203" }
                };
            }
            catch { return null; }
        }
    }
}