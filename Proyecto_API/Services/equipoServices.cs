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
    /// <summary>
    /// Servicio para gestionar las peticiones HTTP relacionadas con equipos
    /// </summary>
    /// <remarks>
    /// Esta clase se encarga de consumir los endpoints de la API de Sportradar
    /// para obtener información de equipos de MotoGP. Implementa caché para
    /// optimizar las peticiones y manejo de errores HTTP.
    /// </remarks>
    public class equipoServices
    {
        private static readonly HttpClient _client = new HttpClient();
        private static Dictionary<string, equiposModels> _cacheEquipos = new Dictionary<string, equiposModels>();

        /// <summary>
        /// Constructor que inicializa el cliente HTTP con las cabeceras necesarias
        /// </summary>
        /// <remarks>
        /// Configura las cabeceras de autenticación (x-api-key), formato de respuesta (accept)
        /// y User-Agent. Verifica que no existan cabeceras duplicadas antes de añadirlas.
        /// </remarks>
        public equipoServices()
        {
            if (!_client.DefaultRequestHeaders.Contains("x-api-key"))
            {
                _client.DefaultRequestHeaders.Add("x-api-key", ApiConfig.ApiKey);
                _client.DefaultRequestHeaders.Add("accept", "application/json");
                _client.DefaultRequestHeaders.Add("User-Agent", "MotoGP_App");
            }
        }

        /// <summary>
        /// Obtiene el perfil completo de un equipo por su ID
        /// </summary>
        /// <param name="idEquipo">Identificador único del equipo en formato Sportradar (ej: sr:competitor:4567)</param>
        /// <returns>Modelo completo del equipo o null si hay error</returns>
        /// <remarks>
        /// Este método utiliza caché para evitar peticiones repetidas.
        /// Consume el endpoint: /teams/{id}/profile.json
        /// Maneja errores HTTP 404, 403, 5xx y errores de red.
        /// </remarks>
        /// <example>
        /// <code>
        /// var servicio = new equipoServices();
        /// var equipo = await servicio.GetEquipoDetalleAsync("sr:competitor:4567");
        /// </code>
        /// </example>
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

        /// <summary>
        /// Obtiene una lista predefinida de IDs de equipos como plan de rescate
        /// </summary>
        /// <returns>Lista de información básica de equipos principales</returns>
        /// <remarks>
        /// Este método se utiliza como plan B en caso de que el endpoint de la temporada
        /// falle en la versión Trial de la API. Devuelve IDs de equipos conocidos.
        /// </remarks>
        /// <example>
        /// <code>
        /// var servicio = new equipoServices();
        /// var equipos = servicio.GetEquiposManual();
        /// </code>
        /// </example>
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