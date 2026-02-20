using Proyecto_API.View;
using System.Windows;

namespace Proyecto_API.Controllers
{
    /// <summary>
    /// Controlador principal que gestiona la navegación entre vistas
    /// </summary>
    /// <remarks>
    /// Este controlador centraliza la lógica de navegación de la aplicación,
    /// permitiendo cambiar entre diferentes vistas sin crear dependencias directas
    /// entre ellas. Implementa el patrón de inyección de dependencias pasando
    /// su propia instancia a las vistas hijas.
    /// </remarks>
    public class MainController
    {
        /// <summary>
        /// Referencia a la ventana principal de la aplicación
        /// </summary>
        /// <remarks>
        /// Permite modificar el contenido de la ventana para navegar entre vistas
        /// </remarks>
        private readonly Window _mainWindow;

        /// <summary>
        /// Constructor que inicializa el controlador con una ventana
        /// </summary>
        /// <param name="mainWindow">Ventana principal sobre la que se realizarán las navegaciones</param>
        /// <remarks>
        /// Guarda la referencia a la ventana principal para poder modificar
        /// su contenido durante la navegación
        /// </remarks>
        public MainController(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        /// <summary>
        /// Navega a la vista de Pilotos
        /// </summary>
        /// <remarks>
        /// Crea una nueva instancia de ViewPilotos pasándole este controlador
        /// como parámetro para mantener la navegación consistente.
        /// Establece la vista como contenido de la ventana principal.
        /// </remarks>
        /// <example>
        /// <code>
        /// var controller = new MainController(this);
        /// controller.NavegarAPilotos();
        /// </code>
        /// </example>
        public void NavegarAPilotos()
        {
            ViewPilotos viewPilotos = new ViewPilotos(this);
            _mainWindow.Content = viewPilotos;
        }

        /// <summary>
        /// Navega a la vista de Equipos
        /// </summary>
        /// <remarks>
        /// Crea una nueva instancia de ViewEquipos pasándole este controlador
        /// como parámetro para mantener la navegación consistente.
        /// Establece la vista como contenido de la ventana principal.
        /// </remarks>
        /// <example>
        /// <code>
        /// var controller = new MainController(this);
        /// controller.NavegarAEquipos();
        /// </code>
        /// </example>
        public void NavegarAEquipos()
        {
            ViewEquipos viewEquipos = new ViewEquipos(this);
            _mainWindow.Content = viewEquipos;
        }

        /// <summary>
        /// Vuelve al menú principal de la aplicación
        /// </summary>
        /// <remarks>
        /// Crea una nueva instancia de MainWindow, la muestra y cierra la ventana actual.
        /// Este método se utiliza típicamente desde las vistas secundarias
        /// cuando el usuario quiere regresar al menú principal.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Desde una vista secundaria
        /// _mainController.VolverAMenuPrincipal();
        /// </code>
        /// </example>
        public void VolverAMenuPrincipal()
        {
            MainWindow main = new MainWindow();
            main.Show();
            _mainWindow.Close();
        }
    }
}
