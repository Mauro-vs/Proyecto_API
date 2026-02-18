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
    /// Lógica de interacción para ViewPilotos.xaml
    /// </summary>
    public partial class ViewPilotos : UserControl
    {
        public ViewPilotos()
        {
            InitializeComponent();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();

            // Cerrar la ventana actual (buscamos la ventana padre de este control)
            Window.GetWindow(this).Close();
        }
    }
}
