using Proyecto_API.Models;
using Proyecto_API.Services;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Proyecto_API.Controllers
{
    /// <summary>
    /// Controlador que gestiona la lógica de negocio de los Equipos
    /// </summary>
    /// <remarks>
    /// Actúa como intermediario entre la capa de servicios (equipoServices)
    /// y la capa de presentación (vistas). Implementa la lógica de negocio
    /// para la obtención de datos de equipos de MotoGP.
    /// </remarks>
    public class EquiposController
    {
        /// <summary>
        /// Servicio encargado de las peticiones HTTP relacionadas con equipos
        /// </summary>
        private readonly equipoServices _servicioEquipos;

        /// <summary>
        /// Constructor que inicializa el controlador con el servicio de equipos
        /// </summary>
        public EquiposController()
        {
            _servicioEquipos = new equipoServices();
        }

        /// <summary>
        /// Carga los equipos de forma asíncrona y notifica a la vista mediante callback
        /// </summary>
        /// <param name="onEquipoCargado">Callback que se ejecuta cada vez que se carga un equipo</param>
        /// <returns>Tarea asíncrona que representa la operación</returns>
        /// <remarks>
        /// Este método implementa carga progresiva (fluida) mediante callbacks.
        /// Utiliza Task.Run para ejecutar las peticiones en un hilo secundario
        /// sin bloquear la interfaz de usuario. Añade un delay de 200ms entre
        /// peticiones para evitar saturar la API.
        /// Utiliza una lista manual de equipos (Plan B) debido a limitaciones
        /// del endpoint de temporada en la versión Trial de la API.
        /// </remarks>
        /// <exception cref="Exception">Lanzada para cualquier error durante la carga</exception>
        /// <example>
        /// <code>
        /// var controller = new EquiposController();
        /// await controller.CargarEquiposAsync(equipo => {
        ///     // Añadir equipo a la lista de la UI
        ///     listaEquipos.Add(equipo);
        /// });
        /// </code>
        /// </example>
        public async Task CargarEquiposAsync(Action<equiposModels> onEquipoCargado)
        {
            try
            {
                // Obtenemos los IDs base (Plan B)
                var listaBase = _servicioEquipos.GetEquiposManual();

                // Separación de hilos (No bloquea la UI)
                await Task.Run(async () =>
                {
                    foreach (var eInfo in listaBase)
                    {
                        var detalle = await _servicioEquipos.GetEquipoDetalleAsync(eInfo.Id);

                        if (detalle != null)
                        {
                            // Avisamos a la Vista en el momento en que se descarga
                            onEquipoCargado(detalle);
                        }

                        await Task.Delay(200);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar equipos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obtiene los detalles específicos de un equipo por su ID
        /// </summary>
        /// <param name="idEquipo">Identificador único del equipo en formato Sportradar</param>
        /// <returns>Modelo completo del equipo o null si hay error</returns>
        /// <remarks>
        /// Demuestra diversidad de endpoints al consumir el perfil detallado de un equipo.
        /// Utiliza el servicio de equipos para obtener la información completa.
        /// Maneja errores y muestra mensajes al usuario en caso de fallo.
        /// </remarks>
        /// <example>
        /// <code>
        /// var controller = new EquiposController();
        /// var equipo = await controller.ObtenerDetalleEquipoAsync("sr:competitor:4567");
        /// if (equipo != null) {
        ///     MessageBox.Show(equipo.Team.Name);
        /// }
        /// </code>
        /// </example>
        public async Task<equiposModels> ObtenerDetalleEquipoAsync(string idEquipo)
        {
            try
            {
                return await _servicioEquipos.GetEquipoDetalleAsync(idEquipo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener detalles del equipo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}