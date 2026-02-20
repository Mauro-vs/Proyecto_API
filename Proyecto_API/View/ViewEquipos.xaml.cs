using Proyecto_API.Controllers;
using Proyecto_API.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_API.View
{
    /// <summary>
    /// Vista que muestra la lista de equipos de MotoGP
    /// </summary>
    /// <remarks>
    /// UserControl que presenta tarjetas de equipos con sus pilotos y colores característicos.
    /// Implementa carga asíncrona progresiva mediante callbacks para mejorar
    /// la experiencia de usuario mostrando cada equipo tan pronto como se descarga.
    /// </remarks>
    public partial class ViewEquipos : UserControl
    {
        /// <summary>
        /// Controlador de lógica de negocio para equipos
        /// </summary>
        private readonly EquiposController _controller;
        
        /// <summary>
        /// Controlador de navegación principal
        /// </summary>
        private readonly MainController _mainController;
        
        /// <summary>
        /// Colección observable de equipos para el binding de datos
        /// </summary>
        /// <remarks>
        /// ObservableCollection permite que la UI se actualice automáticamente
        /// cuando se añaden nuevos equipos
        /// </remarks>
        private ObservableCollection<equiposModels> _listaEquipos;

        /// <summary>
        /// Constructor que inicializa la vista de equipos
        /// </summary>
        /// <param name="mainController">Controlador principal para la navegación</param>
        /// <remarks>
        /// Inicializa los componentes XAML, los controladores necesarios,
        /// la colección de equipos y vincula los datos al control de la interfaz.
        /// Inicia la carga asíncrona de equipos al finalizar la construcción.
        /// Implementa inyección de dependencias recibiendo el MainController.
        /// </remarks>
        public ViewEquipos(MainController mainController)
        {
            InitializeComponent();
            _controller = new EquiposController();
            _mainController = mainController;

            _listaEquipos = new ObservableCollection<equiposModels>();
            ListaEquiposControl.ItemsSource = _listaEquipos;

            CargarEquipos();
        }

        /// <summary>
        /// Carga los equipos de forma asíncrona y los añade progresivamente a la lista
        /// </summary>
        /// <remarks>
        /// Utiliza el controlador para obtener equipos mediante un callback.
        /// Cada equipo se añade a la colección tan pronto como se descarga,
        /// proporcionando feedback visual inmediato al usuario.
        /// Utiliza Dispatcher.Invoke para actualizar la UI desde el hilo secundario.
        /// El método async void es apropiado aquí por ser un manejador de eventos.
        /// </remarks>
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

        /// <summary>
        /// Manejador del evento Click del botón Volver
        /// </summary>
        /// <remarks>
        /// Utiliza el controlador principal para regresar al menú principal,
        /// manteniendo la arquitectura MVC y la separación de responsabilidades
        /// </remarks>
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            // Usamos la arquitectura para volver al menú
            _mainController.VolverAMenuPrincipal();
        }
    }
}