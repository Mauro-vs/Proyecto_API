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
        private static readonly HttpClient _client = new HttpClient();

        // Guardamos los perfiles ya descargados para no repetir llamadas
        private static Dictionary<string, pilotosModels> _cachePilotos = new Dictionary<string, pilotosModels>();

        public pilotoServices()
        {
            if (!_client.DefaultRequestHeaders.Contains("x-api-key"))
            {
                _client.DefaultRequestHeaders.Add("x-api-key", ApiConfig.ApiKey);
                _client.DefaultRequestHeaders.Add("accept", "application/json");
                _client.DefaultRequestHeaders.Add("User-Agent", "MotoGP_App");
            }
        }

        public async Task<pilotosModels> GetSeasonCompetitorsAsync(string idPiloto)
        {
            // Lógica de Caché: Si ya existe en el diccionario, lo devolvemos directamente
            if (_cachePilotos.ContainsKey(idPiloto))
            {
                return _cachePilotos[idPiloto];
            }

            try
            {
                string url = $"{ApiConfig.BaseUrl}/competitors/{idPiloto}/profile.json";
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var piloto = JsonSerializer.Deserialize<pilotosModels>(json, options);

                    // Guardamos en la caché antes de devolverlo
                    if (piloto != null) _cachePilotos[idPiloto] = piloto;

                    return piloto;
                }
                return null;
            }
            catch { return null; }
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