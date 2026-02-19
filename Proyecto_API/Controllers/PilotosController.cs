using Proyecto_API.Models;
using Proyecto_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Proyecto_API.Controllers
{
    /// <summary>
    /// Controlador que gestiona la lógica de negocio de los Pilotos
    /// </summary>
    public class PilotosController
    {
        private readonly pilotoServices _servicioPilotos;

        public PilotosController()
        {
            _servicioPilotos = new pilotoServices();
        }

        /// <summary>
        /// Obtiene la lista de pilotos de la temporada (limitada a 10)
        /// </summary>
        public async Task ObtenerPilotosAsync(Action<pilotosModels> onPilotoCargado)
        {
            try
            {
                var listaBase = await _servicioPilotos.GetAllSeasonPilotosAsync();

                if (listaBase == null)
                    throw new HttpRequestException("Error HTTP: No se pudo obtener la lista de pilotos.");

                var listaReducida = listaBase.Take(10).ToList();

                // utilizamos el task para no bloquear la interfaz mientras se cargan los detalles de cada piloto
                await Task.Run(async () =>
                {
                    foreach (var pInfo in listaReducida)
                    {
                        var perfil = await _servicioPilotos.GetSeasonCompetitorsAsync(pInfo.Id);

                        if (perfil != null)
                        {
                            onPilotoCargado(perfil);
                        }

                        await Task.Delay(200);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener pilotos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obtiene los detalles completos de un piloto específico
        /// </summary>
        public async Task<pilotosModels> ObtenerDetallePilotoAsync(string idPiloto)
        {
            try
            {
                var detalleCompleto = await _servicioPilotos.GetSeasonCompetitorsAsync(idPiloto);
                return detalleCompleto;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener detalles del piloto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Formatea los detalles de un piloto para mostrar
        /// </summary>
        public string FormatearDetallePiloto(pilotosModels piloto)
        {
            if (piloto == null || piloto.Competitor == null)
            {
                return "No se han podido cargar los detalles de este piloto.";
            }

            string nombre = $"{piloto.Competitor.FirstName} {piloto.Competitor.LastName}";
            string pais = piloto.Competitor.CountryCode ?? "Desconocido";
            string equipo = "Sin equipo";

            if (piloto.Teams != null && piloto.Teams.Count > 0)
            {
                equipo = piloto.Teams[0].Name;
            }

            return $"DETALLES DEL PILOTO\n\n" +
                   $"Nombre: {nombre}\n" +
                   $"Nacionalidad: {pais}\n" +
                   $"ID Interno: {piloto.Competitor.Id}\n\n" +
                   $"Equipo Actual: {equipo}\n";
        }
    }
}
