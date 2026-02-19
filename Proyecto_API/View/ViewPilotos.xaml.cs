using Proyecto_API.Models;
using Proyecto_API.Services;
using System.Collections.ObjectModel; // Necesario para la actualización automática
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_API.View
{
    public partial class ViewPilotos : UserControl
    {
        private pilotoServices _servicioPilotos;
        private ObservableCollection<pilotosModels> _listaPilotos;

        public ViewPilotos()
        {
            InitializeComponent();
            _servicioPilotos = new pilotoServices();
            _listaPilotos = new ObservableCollection<pilotosModels>();
            ListaPilotosControl.ItemsSource = _listaPilotos;

            CargarPilotos();
        }

        private async void CargarPilotos()
        {
            // Obtiene la lista base (los IDs)
            var listaBase = await _servicioPilotos.GetAllSeasonPilotosAsync();

            if (listaBase != null)
            {
                // Para evitar bloqueos de la API gratuita, vamos a cargar una cantidad razonable
                var listaReducida = listaBase.Take(10).ToList();

                _listaPilotos.Clear();

                // Gestion de hilos
                await Task.Run(async () =>
                {
                    foreach (var pInfo in listaReducida)
                    {
                        // Pedimos el perfil detallado
                        var perfil = await _servicioPilotos.GetSeasonCompetitorsAsync(pInfo.Id);

                        if (perfil != null)
                        {
                            // Volvemos al hilo de la UI para añadir el piloto
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                _listaPilotos.Add(perfil);
                            });
                        }
                    }
                });
            }
            else
            {
                MessageBox.Show("No se pudo obtener la lista de pilotos.");
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Window.GetWindow(this).Close();
        }
    }
}