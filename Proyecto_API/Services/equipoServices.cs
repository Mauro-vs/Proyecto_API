using Proyecto_API.Config;
using Proyecto_API.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Proyecto_API.Services
{
    public class equipoServices
    {
        private static readonly HttpClient _client = new HttpClient();

        // --- CACHE ---
        private static Dictionary<string, equiposModels> _cacheEquipos = new Dictionary<string, equiposModels>();

        public equipoServices()
        {
            if (!_client.DefaultRequestHeaders.Contains("x-api-key"))
            {
                _client.DefaultRequestHeaders.Add("x-api-key", ApiConfig.ApiKey);
                _client.DefaultRequestHeaders.Add("accept", "application/json");
                _client.DefaultRequestHeaders.Add("User-Agent", "MotoGP_App");
            }
        }

        public async Task<equiposModels> GetEquipoDetalleAsync(string idEquipo)
        {
            if (_cacheEquipos.ContainsKey(idEquipo)) return _cacheEquipos[idEquipo];

            try
            {
                string url = $"{ApiConfig.BaseUrl}/teams/{idEquipo}/profile.json";
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var equipo = JsonSerializer.Deserialize<equiposModels>(json, options);

                    if (equipo != null) _cacheEquipos[idEquipo] = equipo;

                    return equipo;
                }
                else
                {
                    int statusCode = (int)response.StatusCode;

                    if (statusCode == 404)
                    {
                        MessageBox.Show($"Error 404: No se ha encontrado el equipo con ID {idEquipo}.");
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
            catch
            {
                MessageBox.Show("Error de red: Comprueba tu conexión a internet.");
            }
            return null;
        }

        // Plan de rescate por si el endpoint de temporada falla en Trial
        public List<equipoInfo> GetEquiposManual()
        {
            return new List<equipoInfo>
            {
                new equipoInfo { Id = "sr:competitor:4567" },
                new equipoInfo { Id = "sr:competitor:22035" },
                new equipoInfo { Id = "sr:competitor:22033" },
                new equipoInfo { Id = "sr:competitor:22037" },
                new equipoInfo { Id = "sr:competitor:22039" },
            };
        }
    }
}