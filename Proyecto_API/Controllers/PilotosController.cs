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
    /// <remarks>
    /// Actúa como intermediario entre la capa de servicios (pilotoServices)
    /// y la capa de presentación (vistas). Implementa la lógica de negocio
    /// para la obtención y formateo de datos de pilotos de MotoGP.
    /// </remarks>
    public class PilotosController
    {
        /// <summary>
        /// Servicio encargado de las peticiones HTTP relacionadas con pilotos
        /// </summary>
        private readonly pilotoServices _servicioPilotos;

        /// <summary>
        /// Constructor que inicializa el controlador con el servicio de pilotos
        /// </summary>
        public PilotosController()
        {
            _servicioPilotos = new pilotoServices();
        }

        /// <summary>
        /// Obtiene la lista de pilotos de la temporada (limitada a 10) de forma asíncrona
        /// </summary>
        /// <param name="onPilotoCargado">Callback que se ejecuta cada vez que se carga un piloto</param>
        /// <returns>Tarea asíncrona que representa la operación</returns>
        /// <remarks>
        /// Este método implementa carga progresiva mediante callbacks para no bloquear la UI.
        /// Utiliza Task.Run para ejecutar las peticiones en un hilo secundario.
        /// Añade un delay de 200ms entre peticiones para evitar saturar la API.
        /// </remarks>
        /// <exception cref="HttpRequestException">Lanzada cuando falla la petición HTTP</exception>
        /// <exception cref="Exception">Lanzada para cualquier otro error durante el proceso</exception>
        /// <example>
        /// <code>
        /// var controller = new PilotosController();
        /// await controller.ObtenerPilotosAsync(piloto => {
        ///     // Añadir piloto a la lista de la UI
        ///     listaPilotos.Add(piloto);
        /// });
        /// </code>
        /// </example>
        public async Task ObtenerPilotosAsync(Action<pilotosModels> onPilotoCargado)
        {
            try
            {
                var listaBase = await _servicioPilotos.GetAllSeasonPilotosAsync();

                if (listaBase == null)
                    throw new HttpRequestException("Error HTTP: No se pudo obtener la lista de pilotos.");

                var listaReducida = listaBase.Take(6).ToList();

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
        /// <param name="idPiloto">Identificador único del piloto en formato Sportradar</param>
        /// <returns>Modelo completo del piloto o null si hay error</returns>
        /// <remarks>
        /// Utiliza el servicio de pilotos para obtener el perfil completo.
        /// Maneja errores y muestra mensajes al usuario en caso de fallo.
        /// </remarks>
        /// <example>
        /// <code>
        /// var controller = new PilotosController();
        /// var piloto = await controller.ObtenerDetallePilotoAsync("sr:competitor:21999");
        /// if (piloto != null) {
        ///     MessageBox.Show(piloto.Competitor.Name);
        /// }
        /// </code>
        /// </example>
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
        /// Formatea los detalles de un piloto en un string legible para mostrar al usuario
        /// </summary>
        /// <param name="piloto">Modelo del piloto a formatear</param>
        /// <returns>String formateado con los detalles del piloto</returns>
        /// <remarks>
        /// Genera un texto estructurado con información del piloto incluyendo:
        /// nombre completo, nacionalidad, ID interno y equipo actual.
        /// Si algún dato no está disponible, muestra valores predeterminados.
        /// </remarks>
        /// <example>
        /// <code>
        /// var controller = new PilotosController();
        /// var mensaje = controller.FormatearDetallePiloto(piloto);
        /// MessageBox.Show(mensaje);
        /// </code>
        /// </example>
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
