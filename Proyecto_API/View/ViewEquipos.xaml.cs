using Proyecto_API.Controllers;
using Proyecto_API.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_API.View
{
    public partial class ViewEquipos : UserControl
    {
        private readonly EquiposController _controller;
        private readonly MainController _mainController;
        private ObservableCollection<equiposModels> _listaEquipos;

        public ViewEquipos(MainController mainController)
        {
            InitializeComponent();
            _controller = new EquiposController();
            _mainController = mainController;

            _listaEquipos = new ObservableCollection<equiposModels>();
            ListaEquiposControl.ItemsSource = _listaEquipos;

            CargarEquipos();
        }

        private async void CargarEquipos()
        {
            // Llamamos al controlador asíncrono
            await _controller.CargarEquiposAsync(equipo =>
            {
                // Volvemos al hilo de la pantalla para añadir la tarjeta
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _listaEquipos.Add(equipo);
                });
            });
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            // Usamos la arquitectura para volver al menú
            _mainController.VolverAMenuPrincipal();
        }
    }
}