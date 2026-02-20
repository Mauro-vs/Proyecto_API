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
    /// <summary>
    /// Servicio para gestionar las peticiones HTTP relacionadas con pilotos
    /// </summary>
    /// <remarks>
    /// Esta clase se encarga de consumir los endpoints de la API de Sportradar
    /// para obtener información de pilotos de MotoGP. Implementa caché para
    /// optimizar las peticiones y manejo de errores HTTP.
    /// </remarks>
    public class pilotoServices
    {
        private readonly HttpClient _client = new HttpClient();
        private static Dictionary<string, pilotosModels> _cache = new Dictionary<string, pilotosModels>();

        /// <summary>
        /// Constructor que inicializa el cliente HTTP con las cabeceras necesarias
        /// </summary>
        /// <remarks>
        /// Configura las cabeceras de autenticación (x-api-key) y formato de respuesta (accept).
        /// Verifica que no existan cabeceras duplicadas antes de añadirlas.
        /// </remarks>
        public pilotoServices()
        {
            // si ya tiene la cabecera, no la añade (evita duplicados)
            if (!_client.DefaultRequestHeaders.Contains("x-api-key"))
            {
                _client.DefaultRequestHeaders.Add("x-api-key", ApiConfig.ApiKey);
                _client.DefaultRequestHeaders.Add("accept", "application/json");
            }
        }

        /// <summary>
        /// Obtiene el perfil completo de un piloto por su ID
        /// </summary>
        /// <param name="idPiloto">Identificador único del piloto en formato Sportradar (ej: sr:competitor:21999)</param>
        /// <returns>Modelo completo del piloto o null si hay error</returns>
        /// <remarks>
        /// Este método utiliza caché para evitar peticiones repetidas.
        /// Consume el endpoint: /competitors/{id}/profile.json
        /// Maneja errores HTTP 404, 403, 5xx y errores de red.
        /// </remarks>
        /// <example>
        /// <code>
        /// var servicio = new pilotoServices();
        /// var piloto = await servicio.GetSeasonCompetitorsAsync("sr:competitor:21999");
        /// </code>
        /// </example>
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

        /// <summary>
        /// Obtiene la lista de todos los pilotos de la temporada actual
        /// </summary>
        /// <returns>Lista de información básica de pilotos o null si hay error</returns>
        /// <remarks>
        /// Consume el endpoint: /seasons/{SeasonId}/competitors.json
        /// En caso de error, devuelve una lista de pilotos predefinidos como plan de rescate.
        /// </remarks>
        /// <example>
        /// <code>
        /// var servicio = new pilotoServices();
        /// var pilotos = await servicio.GetAllSeasonPilotosAsync();
        /// </code>
        /// </example>
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

                // Plan de rescate con pilotos predefinidos
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