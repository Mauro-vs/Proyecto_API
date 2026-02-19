using Proyecto_API.Models;
using Proyecto_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proyecto_API.View
{
    /// <summary>
    /// Lógica de interacción para ViewEquipos.xaml
    /// </summary>
    public partial class ViewEquipos : UserControl
    {
        private equipoServices _servicioEquipos;
        private List<equiposModels> _listaEquipos;
        public ViewEquipos()
        {
            InitializeComponent();
            _servicioEquipos = new equipoServices();
            _listaEquipos = new List<equiposModels>();
            CargarEquipos();
        }

        private async void CargarEquipos()
        {
            // Usamos la lista manual para evitar errores de "Package" en el Trial
            var listaBase = _servicioEquipos.GetEquiposManual();

            // --- SEPARACION DE HILOS ---
            await Task.Run(async () => {
                foreach (var eInfo in listaBase)
                {
                    var detalle = await _servicioEquipos.GetEquipoDetalleAsync(eInfo.Id);

                    if (detalle != null)
                    {
                        // Volvemos al hilo de la UI para pintar
                        App.Current.Dispatcher.Invoke(() => {
                            _listaEquipos.Add(detalle);
                            ListaEquiposControl.ItemsSource = null;
                            ListaEquiposControl.ItemsSource = _listaEquipos;
                        });
                    }
                }
            });
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            // Vuelve a cargar la ventana principal
            MainWindow main = new MainWindow();
            main.Show();

            // Cierra la ventana actual
            Window.GetWindow(this).Close();
        }
    }
}
