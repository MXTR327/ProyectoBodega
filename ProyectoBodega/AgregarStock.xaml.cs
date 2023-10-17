using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class AgregarStock : Window
    {
        private string filtro;
        CN_VentanaAgregarStock cn_ventanaAgregarStock = new CN_VentanaAgregarStock();
        public AgregarStock()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            filtro = "";
            CargarProducto();
            if (dgProducto.Items.Count > 0)
            {
                dgProducto.SelectedIndex = 0;
            }

        }

        private void CargarProducto()
        {
            DataTable dt = cn_ventanaAgregarStock.tblProducto(filtro);
            dgProducto.ItemsSource = dt.DefaultView;

            
        }

        private void txtBuscadorProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            filtro = txtBuscadorProducto.Text;
            CargarProducto(); 
            if (dgProducto.Items.Count > 0)
            {
                dgProducto.SelectedIndex = 0;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtStock_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbCantidad.Visibility = Visibility.Collapsed;
        }

        private void txtStock_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCantidad.Text))
            {
                txtbCantidad.Visibility = Visibility.Visible;
            }
        }

        private void txtStock_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) && e.Key != Key.Back || (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                e.Handled = true;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtBuscadorProducto_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbBuscar.Visibility = Visibility.Collapsed;
        }

        private void txtBuscadorProducto_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscadorProducto.Text))
            {
                txtbBuscar.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void dgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView filaSeleccionada = (DataRowView)dgProducto.SelectedItem;

            if (filaSeleccionada != null)
            {
                int stock = Convert.ToInt32(filaSeleccionada["stock"]);

                txtStockInicial.Text = stock.ToString();
                txtStockFinal.Text = stock.ToString();
            }
        }

        private void txtStock_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtCantidad.Text, out int cantidad) && int.TryParse(txtStockInicial.Text, out int stockInicial))
            {
                int stockFinal = stockInicial + cantidad;
                txtStockFinal.Text = stockFinal.ToString();
                if (double.Parse(txtStockFinal.Text) > double.Parse(txtStockInicial.Text))
                {
                    txtStockFinal.Foreground = Brushes.Green;
                }
            }
            else
            {
                txtStockFinal.Text = txtStockInicial.Text;
                txtStockFinal.Foreground = Brushes.Red;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
