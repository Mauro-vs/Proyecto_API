using Proyecto_API.Controllers;
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
        private readonly MainController _controller;

        public MainWindow()
        {
            InitializeComponent();
            _controller = new MainController(this);
        }

        private void ButtonPilotos_Click(object sender, RoutedEventArgs e)
        {
            _controller.NavegarAPilotos();
        }

        private void ButtonEquipos_Click(object sender, RoutedEventArgs e)
        {
            _controller.NavegarAEquipos();
        }
    }
}
