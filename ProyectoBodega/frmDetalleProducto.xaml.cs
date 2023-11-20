using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace ProyectoBodega
{
    public partial class frmDetalleProducto : Window
    {
        internal index ventanaIndex;

        public string Id, nombre;
        public int stock, cantidad;
        public decimal precio;
        public frmDetalleProducto()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ventanaIndex != null)
            {
                DataRowView filaSeleccionada = (DataRowView)ventanaIndex.dgProducto.SelectedItem;

                Id = filaSeleccionada["idProducto"].ToString();
                nombre = filaSeleccionada["nombre_producto"].ToString();
                precio = Convert.ToDecimal(filaSeleccionada["precio_venta"]);
                stock = Convert.ToInt32(filaSeleccionada["stock"]);

                txtID.Text = Id;
                txtNombre.Text = nombre;
                txtPrecio.Text = precio.ToString("F2", CultureInfo.InvariantCulture);
                txtStockInicial.Text = stock.ToString();
                txtStockFinal.Text = stock.ToString();
            }
            txtCantidad.Focus();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void InsertarNumero(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button boton)) return;

            txtCantidad.Focus();
            if (boton.Content.ToString() == "CE")
            {
                txtCantidad.Text = string.Empty;
            }
            else if (boton.Content.ToString() == "C")
            {
                if (txtCantidad.Text.Length > 0)
                {
                    txtCantidad.Text = txtCantidad.Text.Remove(txtCantidad.Text.Length - 1, 1);
                    txtCantidad.CaretIndex = txtCantidad.Text.Length;
                }
            }
            else
            {
                string textoDelBoton = boton.Content.ToString();
                txtCantidad.Text = txtCantidad.Text + textoDelBoton;
                txtCantidad.CaretIndex = txtCantidad.Text.Length;
            }
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnCambiarPrecio_Click(object sender, RoutedEventArgs e)
        {
            txtPrecio.IsReadOnly = !txtPrecio.IsReadOnly;
            
            if (txtPrecio.IsReadOnly == false)
            {
                txtPrecio.Focus();
                txtbPrecio.Cursor = Cursors.IBeam;
            }
            else
            {
                txtbPrecio.Cursor = Cursors.Arrow;
            }
        }
        private void txtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(txtCantidad.Text, out int cantidad) || !int.TryParse(txtStockInicial.Text, out int stockInicial))
            {
                txtStockFinal.Text = txtStockInicial.Text;
                return;
            }

            int stockFinal = stockInicial - cantidad;
            txtStockFinal.Text = stockFinal.ToString();

            txtStockFinal.Foreground = double.Parse(txtStockFinal.Text) < 0 ? Brushes.Red : (Brush)Brushes.Black;
        }
        private void txtCantidad_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape) return;

            if (!char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) && e.Key != Key.Back || (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                e.Handled = true;
            }
        }
        private void txtPrecio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string text = textBox.Text;
            int cursorIndex = textBox.SelectionStart;
            int indexOfDot = text.IndexOf('.');
            bool dotPressed = e.Key == Key.OemPeriod || e.Key == Key.Decimal;
            if (e.Key == Key.Enter || e.Key == Key.Escape || e.Key == Key.Tab || e.Key == Key.Back ||
                e.Key == Key.Left || e.Key == Key.Right)
            {
                return;
            }
            bool isValidInput = (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || dotPressed;
            if (!isValidInput)
            {
                e.Handled = true;
                return;
            }
            bool textContainsDot = text.Contains(".");
            if (dotPressed && textContainsDot)
            {
                e.Handled = true;
                return;
            }
            if (dotPressed)
            {
                bool isValidDotPosition = (cursorIndex > 0 && cursorIndex == text.Length);
                e.Handled = !isValidDotPosition;
                return;
            }
            if (indexOfDot != -1 && cursorIndex > indexOfDot)
            {
                string decimalPart = text.Substring(indexOfDot + 1);
                if (decimalPart.Length >= 2 && e.Key != Key.Back)
                {
                    e.Handled = true;
                    return;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnContinuar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPrecio.Text) || txtPrecio.Text==",")
            {
                MessageBox.Show("Especifique un precio del Producto", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtCantidad.Text) || int.Parse(txtCantidad.Text) <= 0)
            {
                MessageBox.Show("Especifique una cantidad a vender", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (int.Parse(txtStockInicial.Text) > 0 && int.Parse(txtStockFinal.Text) >= 0)
            {
                cantidad = int.Parse(txtCantidad.Text);

                Seleccionar();
                ventanaIndex.CalcularSumaTotal();
                ventanaIndex.ContadorProductosVenta();
            }
            else
            {
                MessageBox.Show("No tiene suficientes productos", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtPrecio.Text = precio.ToString("F2", CultureInfo.InvariantCulture);
        }
        private void Seleccionar()
        {
            DataRow nuevaFila = ventanaIndex.tablaVenta.NewRow();
            nuevaFila["idProducto"] = Id;
            nuevaFila["nombre_producto"] = nombre;
            nuevaFila["precio_compra"] = txtPrecio.Text;
            nuevaFila["Cantidad"] = cantidad;
            if (decimal.TryParse(txtPrecio.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal Precio))
            {
                decimal Total = cantidad * Precio;
                nuevaFila["Total"] = Total.ToString("F2", CultureInfo.InvariantCulture);
            }
            ventanaIndex.tablaVenta.Rows.Add(nuevaFila);

            Close();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Arrastrar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Ventana.Cursor = Cursors.SizeAll;
                DragMove();
                Ventana.Cursor = Cursors.Arrow;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Ventana_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close(); 
            else if (e.Key == Key.Enter) btnContinuar_Click(sender, e);
        }
    }
}
