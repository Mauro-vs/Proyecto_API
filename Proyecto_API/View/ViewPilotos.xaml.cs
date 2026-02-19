using Proyecto_API.Models;
using Proyecto_API.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_API.View
{
    public partial class ViewPilotos : UserControl
    {
        private pilotoServices _servicioPilotos;
        // Cambio: La lista debe ser de pilotosModels (el DTO principal)
        private List<pilotosModels> _listaPilotos;

        public ViewPilotos()
        {
            InitializeComponent();
            _servicioPilotos = new pilotoServices();
            _listaPilotos = new List<pilotosModels>();
            CargarPilotos();
        }

        private async void CargarPilotos()
        {
            // 1. Obtenemos los IDs (Hilo principal, pero asíncrono)
            var listaBase = await _servicioPilotos.GetAllSeasonPilotosAsync();

            if (listaBase != null)
            {
                var listaReducida = listaBase.Take(10).ToList();
                _listaPilotos.Clear();

                // 2. Ejecutamos la carga en un hilo separado (Punto: Separació de fils)
                await Task.Run(async () => {
                    foreach (var pInfo in listaReducida)
                    {
                        // Descargamos el dato
                        var perfil = await _servicioPilotos.GetSeasonCompetitorsAsync(pInfo.Id);

                        if (perfil != null)
                        {
                            // 3. Para modificar la lista que ve el usuario, volvemos al hilo de la UI
                            App.Current.Dispatcher.Invoke(() => {
                                _listaPilotos.Add(perfil);
                                ListaPilotosControl.ItemsSource = null;
                                ListaPilotosControl.ItemsSource = _listaPilotos;
                            });
                        }
                    }
                });
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