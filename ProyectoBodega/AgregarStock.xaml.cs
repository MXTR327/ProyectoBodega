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
        internal index ventanaIndex;

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
                txtCantidad.Text = "";
                txtbCantidad.Visibility = Visibility.Visible;
            }
            else
            {
                txtbCantidad.Visibility = Visibility.Collapsed;
                txtCantidad.Text = "Sin productos";
                txtStockInicial.Text = "0";
                txtStockFinal.Text = "0";
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
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                return;
            }
            if (!(txtCantidad.Text.Contains("-")))
            {
                if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
                {
                    return;
                }
            }
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
                txtCantidad.Focus();
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
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducto.Items.Count == 0)
                return;

            DataRowView filaSeleccionada = (DataRowView)dgProducto.SelectedItem;

            if (filaSeleccionada == null)
                return;

            int indexActual = dgProducto.Items.IndexOf(filaSeleccionada);
            int stockInicial = int.Parse(txtStockInicial.Text);
            int stockFinal = int.Parse(txtStockFinal.Text);
            

            if (stockInicial == stockFinal || stockFinal < 0 || string.IsNullOrEmpty(txtCantidad.Text))
            {
                MessageBox.Show("Cantidad no válida", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtCantidad.Focus();
                return;
            }
            int id = Convert.ToInt32(filaSeleccionada["idProducto"]);
            int cantidad = int.Parse(txtCantidad.Text);

            MessageBoxResult result = MessageBox.Show($"¿Está seguro de {(cantidad < 0 ? "disminuir" : "aumentar")} {Math.Abs(cantidad)} al stock?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            cn_ventanaAgregarStock.idProducto = id;
            cn_ventanaAgregarStock.cantidadAgregar = txtCantidad.Text;

            bool rpta = cn_ventanaAgregarStock.AgregarCantidad();

            if (rpta)
            {
                filtro = txtBuscadorProducto.Text;
                CargarProducto();
                dgProducto.SelectedIndex = indexActual;
                txtCantidad.Text = "";
                MessageBox.Show("El stock se actualizó correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                if (ventanaIndex != null)
                {
                    ventanaIndex.CargarProducto();
                }
            }
            else
            {
                MessageBox.Show("Ocurrió un error intentando subir a la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
            else if(e.Key == Key.Enter)
            {
                btnAgregar_Click(sender, e);
            }
        }
    }
}
