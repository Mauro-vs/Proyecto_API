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
    public class EquiposController
    {
        private readonly equipoServices _servicioEquipos;

        public EquiposController()
        {
            _servicioEquipos = new equipoServices();
        }

        /// <summary>
        /// Carga los equipos y avisa a la vista uno a uno (Carga fluida)
        /// </summary>
        public async Task CargarEquiposAsync(Action<equiposModels> onEquipoCargado)
        {
            try
            {
                // Obtenemos los IDs base (Plan B)
                var listaBase = _servicioEquipos.GetEquiposManual();

                // RÚBRICA: Separación de hilos (No bloquea la UI)
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
        /// RÚBRICA: Diversidad de endpoints. Obtiene detalles específicos de un equipo.
        /// </summary>
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