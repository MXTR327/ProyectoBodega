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
    public partial class CuadroDialogo : Window
    {
        public string ValorIngresado { get; private set; }
        public string titulo="Sin titulo";
        public CuadroDialogo()
        {
            InitializeComponent();
        }
        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                MessageBox.Show("No puede dejar campos vacios","Alerta");
                txtUsuario.Focus();
                return;
            }
            if (lblTitulo.Content.ToString() != "Sin titulo")
            {
                ValorIngresado = txtUsuario.Text;
                DialogResult = true;
            }
        }
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (lblTitulo.Content.ToString() == "Sin titulo")
            {
                Close();
            }
            else
            {
                ValorIngresado = null;
                DialogResult = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblTitulo.Content = titulo;
            txtUsuario.Focus();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        
        private void txtUsuario_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbUsuario.Visibility = Visibility.Collapsed;
        }

        private void txtUsuario_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text)) txtbUsuario.Visibility = Visibility.Visible;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) Aceptar_Click(sender, e);
            else if (e.Key == Key.Escape) Cancelar_Click(sender, e);
        }
    }
}
