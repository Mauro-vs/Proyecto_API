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

        // Este método obtiene el perfil detallado de un piloto por su ID
        public async Task<pilotosModels> GetSeasonCompetitorsAsync(string idPiloto)
        {
            if (_cache.ContainsKey(idPiloto)) return _cache[idPiloto];

            try
            {
                string url = $"{ApiConfig.BaseUrl}/competitors/{idPiloto}/profile.json";
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var piloto = JsonSerializer.Deserialize<pilotosModels>(json, options);

                    if (piloto != null) _cache[idPiloto] = piloto;

                    return piloto;
                }
                else
                {
                    // --- MANEJO DE HTTP Y RED ---
                    int statusCode = (int)response.StatusCode;

                    if (statusCode == 404) 
                    {
                        MessageBox.Show($"Error 404: No se ha encontrado el piloto con ID {idPiloto}.");
                    }
                    else if (statusCode == 403) 
                    {
                        MessageBox.Show("Error 403: Permiso denegado. Revisa tu API Key.");
                    }
                    else if (statusCode >= 500)
                    {
                        MessageBox.Show($"Error {statusCode}: El servidor de la API está fallando.");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error de conexión: Comprueba tu conexión a internet.");
            }
            return null;
        }

        // Este método obtiene la lista de pilotos de la temporada actual
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

                return new List<pilotosInfo>
                {
                    new pilotosInfo { Id = "sr:competitor:21999" },
                    new pilotosInfo { Id = "sr:competitor:21997" },
                    new pilotosInfo { Id = "sr:competitor:8003" },
                    new pilotosInfo { Id = "sr:competitor:34971" },
                };
            }
            catch 
            { 
                MessageBox.Show("Error de red: No se pudo contactar con la API.");
            }
            return null;
        }
    }
}