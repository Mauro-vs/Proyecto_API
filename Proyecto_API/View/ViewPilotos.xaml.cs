using Proyecto_API.Controllers;
using Proyecto_API.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_API.View
{
    /// <summary>
    /// Vista que muestra la lista de pilotos de MotoGP
    /// </summary>
    /// <remarks>
    /// UserControl que presenta tarjetas de pilotos con información detallada.
    /// Implementa carga asíncrona progresiva para mejorar la experiencia de usuario.
    /// Permite visualizar detalles adicionales al hacer clic en las tarjetas.
    /// </remarks>
    public partial class ViewPilotos : UserControl
    {
        /// <summary>
        /// Controlador de lógica de negocio para pilotos
        /// </summary>
        private readonly PilotosController _controller;
        
        /// <summary>
        /// Controlador de navegación principal
        /// </summary>
        private readonly MainController _mainController;
        
        /// <summary>
        /// Colección observable de pilotos para el binding de datos
        /// </summary>
        /// <remarks>
        /// ObservableCollection permite que la UI se actualice automáticamente
        /// cuando se añaden nuevos pilotos
        /// </remarks>
        private ObservableCollection<pilotosModels> _listaPilotos;

        /// <summary>
        /// Constructor que inicializa la vista de pilotos
        /// </summary>
        /// <param name="mainController">Controlador principal para la navegación</param>
        /// <remarks>
        /// Inicializa los componentes XAML, los controladores necesarios,
        /// la colección de pilotos y vincula los datos al control de la interfaz.
        /// Inicia la carga asíncrona de pilotos al finalizar la construcción.
        /// Implementa inyección de dependencias recibiendo el MainController.
        /// </remarks>
        public ViewPilotos(MainController mainController)
        {
            InitializeComponent();
            _controller = new PilotosController();
            _mainController = mainController;
            _listaPilotos = new ObservableCollection<pilotosModels>();
            ListaPilotosControl.ItemsSource = _listaPilotos;

            CargarPilotos();
        }

        /// <summary>
        /// Carga los pilotos de forma asíncrona y los añade progresivamente a la lista
        /// </summary>
        /// <remarks>
        /// Utiliza el controlador para obtener pilotos mediante un callback.
        /// Cada piloto se añade a la colección tan pronto como se descarga,
        /// proporcionando feedback visual inmediato al usuario.
        /// El método async void es apropiado aquí por ser un manejador de eventos.
        /// </remarks>
        private async void CargarPilotos()
        {
            // Delega la lógica al controlador
            await _controller.ObtenerPilotosAsync(piloto =>
            {
                // Volvemos al hilo de la UI para actualizar la interfaz
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _listaPilotos.Add(piloto);
                });
            });
        }

        /// <summary>
        /// Manejador del evento Click en una tarjeta de piloto
        /// </summary>
        /// <remarks>
        /// Obtiene el ID del piloto desde la propiedad Tag del Border,
        /// solicita los detalles completos al controlador y los muestra
        /// en un MessageBox formateado. Implementa validación de datos
        /// antes de mostrar la información.
        /// </remarks>
        private async void TarjetaPiloto_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // 1. Obtenemos el borde que se ha clicado y sacamos el ID del piloto
            if (sender is Border tarjeta && tarjeta.Tag is string idPiloto)
            {
                // 2. Usamos el controlador para obtener los detalles
                var detalleCompleto = await _controller.ObtenerDetallePilotoAsync(idPiloto);

                if (detalleCompleto != null && detalleCompleto.Competitor != null)
                {
                    // 3. Usamos el controlador para formatear el mensaje
                    string mensaje = _controller.FormatearDetallePiloto(detalleCompleto);
                    MessageBox.Show(mensaje, "Perfil Completo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se han podido cargar los detalles de este piloto.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Manejador del evento Click del botón Volver
        /// </summary>
        /// <remarks>
        /// Utiliza el controlador principal para regresar al menú principal,
        /// manteniendo la separación de responsabilidades
        /// </remarks>
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            _mainController.VolverAMenuPrincipal();
        }
    }
}