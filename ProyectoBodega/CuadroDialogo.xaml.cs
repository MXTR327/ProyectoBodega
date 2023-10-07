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
using System.Windows.Shapes;

namespace ProyectoBodega
{
    /// <summary>
    /// Lógica de interacción para CuadroDialogo.xaml
    /// </summary>
    public partial class CuadroDialogo : Window
    {
        public string ValorIngresado { get; private set; }
        public CuadroDialogo()
        {
            InitializeComponent();
        }
        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            ValorIngresado = InputTextBox.Text;
            DialogResult = true;
        }
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            ValorIngresado = null;
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InputTextBox.Focus();
        }
    }
}
