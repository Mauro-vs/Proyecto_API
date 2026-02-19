using Proyecto_API.View;
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

namespace Proyecto_API
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonPilotos_Click(object sender, RoutedEventArgs e)
        {
            ViewPilotos windowPilotos = new ViewPilotos();
            this.Content = windowPilotos;
        }

        private void ButtonEquipos_Click(object sender, RoutedEventArgs e)
        {
            ViewEquipos windowEquipos = new ViewEquipos();
            this.Content = windowEquipos;
        }

        private void ButtonCircuitos_Click(object sender, RoutedEventArgs e)
        {
            ViewCircuitos windowCircuitos = new ViewCircuitos();
            this.Content = windowCircuitos;
        }

        private void ButtonCalendario_Click(object sender, RoutedEventArgs e)
        {
            ViewCalendario windowCarreras = new ViewCalendario();
            this.Content = windowCarreras;
        }
    }
}
