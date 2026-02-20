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
    /// Ventana principal de la aplicación MotoGP Dashboard
    /// </summary>
    /// <remarks>
    /// Esta es la ventana de entrada que muestra el menú principal con acceso
    /// a las diferentes secciones de la aplicación (Pilotos, Equipos, etc.).
    /// Implementa el patrón MVC utilizando MainController para la navegación.
    /// </remarks>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Controlador principal para la gestión de navegación
        /// </summary>
        private readonly MainController _controller;

        /// <summary>
        /// Constructor que inicializa la ventana principal
        /// </summary>
        /// <remarks>
        /// Inicializa los componentes de la interfaz (XAML) y crea
        /// una instancia del controlador principal pasándose a sí misma
        /// como parámetro para permitir la navegación.
        /// </remarks>
        public MainWindow()
        {
            InitializeComponent();
            _controller = new MainController(this);
        }

        /// <summary>
        /// Manejador del evento Click del botón de Pilotos
        /// </summary>
        /// <remarks>
        /// Delega la navegación al controlador principal para mantener
        /// la separación de responsabilidades
        /// </remarks>
        private void ButtonPilotos_Click(object sender, RoutedEventArgs e)
        {
            _controller.NavegarAPilotos();
        }

        /// <summary>
        /// Manejador del evento Click del botón de Equipos
        /// </summary>
        /// <remarks>
        /// Delega la navegación al controlador principal para mantener
        /// la separación de responsabilidades
        /// </remarks>
        private void ButtonEquipos_Click(object sender, RoutedEventArgs e)
        {
            _controller.NavegarAEquipos();
        }
    }
}
