using Proyecto_API.Models;
using Proyecto_API.Services;
using System;
using System.Collections.ObjectModel; // <--- Añadir esto
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_API.View
{
    public partial class ViewEquipos : UserControl
    {
        private equipoServices _servicioEquipos;
        private ObservableCollection<equiposModels> _listaEquipos;

        public ViewEquipos()
        {
            InitializeComponent();
            _servicioEquipos = new equipoServices();
            _listaEquipos = new ObservableCollection<equiposModels>();
            ListaEquiposControl.ItemsSource = _listaEquipos;

            CargarEquipos();
        }

        private async void CargarEquipos()
        {
            var listaBase = _servicioEquipos.GetEquiposManual();

            // Gestion de hilos
            await Task.Run(async () =>
            {
                foreach (var eInfo in listaBase)
                {
                    var detalle = await _servicioEquipos.GetEquipoDetalleAsync(eInfo.Id);

                    if (detalle != null)
                    {
                        // Volvemos al hilo de la UI para añadir a la colección
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            // Al ser ObservableCollection, la UI se entera sola
                            // y añade la tarjeta al momento sin hacer nada más
                            _listaEquipos.Add(detalle);
                        });
                    }
                }
            });
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Window.GetWindow(this).Close();
        }
    }
}