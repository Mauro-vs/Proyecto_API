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
    /// Lógica de interacción para ViewCalendario.xaml
    /// </summary>
    public partial class ViewCalendario : UserControl
    {
        public ViewCalendario()
        {
            InitializeComponent();
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
