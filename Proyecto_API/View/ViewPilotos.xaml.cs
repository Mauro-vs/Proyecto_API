using Proyecto_API.Controllers;
using Proyecto_API.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_API.View
{
    public partial class ViewPilotos : UserControl
    {
        private readonly PilotosController _controller;
        private readonly MainController _mainController;
        private ObservableCollection<pilotosModels> _listaPilotos;

        public ViewPilotos(MainController mainController)
        {
            InitializeComponent();
            _controller = new PilotosController();
            _mainController = mainController;
            _listaPilotos = new ObservableCollection<pilotosModels>();
            ListaPilotosControl.ItemsSource = _listaPilotos;

            CargarPilotos();
        }

        private async void CargarPilotos()
        {
            // Llamamos al controlador y le decimos: "Cada vez que tengas un piloto, haz esto:"
            await _controller.ObtenerPilotosAsync(piloto =>
            {
                // Volvemos al hilo de la pantalla para añadir la tarjeta
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _listaPilotos.Add(piloto);
                });
            });
        }

        // muestra el detalle del piloto al hacer click en su tarjeta
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

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            _mainController.VolverAMenuPrincipal();
        }
    }
}