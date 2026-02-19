using Proyecto_API.View;
using System.Windows;

namespace Proyecto_API.Controllers
{
    /// <summary>
    /// Controlador principal que gestiona la navegación entre vistas
    /// </summary>
    public class MainController
    {
        private readonly Window _mainWindow;

        public MainController(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        /// <summary>
        /// Navega a la vista de Pilotos
        /// </summary>
        public void NavegarAPilotos()
        {
            ViewPilotos viewPilotos = new ViewPilotos(this);
            _mainWindow.Content = viewPilotos;
        }

        /// <summary>
        /// Navega a la vista de Equipos
        /// </summary>
        public void NavegarAEquipos()
        {
            ViewEquipos viewEquipos = new ViewEquipos(this);
            _mainWindow.Content = viewEquipos;
        }

        /// <summary>
        /// Vuelve al menú principal
        /// </summary>
        public void VolverAMenuPrincipal()
        {
            MainWindow main = new MainWindow();
            main.Show();
            _mainWindow.Close();
        }
    }
}
