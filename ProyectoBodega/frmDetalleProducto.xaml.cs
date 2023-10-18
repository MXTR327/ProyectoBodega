using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProyectoBodega
{
    public partial class frmDetalleProducto : Window
    {
        internal index ventanaIndex;

        public string Id, nombre;
        public int stock, cantidad;
        public double precio;
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
                precio = Convert.ToDouble(filaSeleccionada["precio_venta"]);
                stock = Convert.ToInt32(filaSeleccionada["stock"]);

                txtID.Text = Id;
                txtNombre.Text = nombre;
                txtPrecio.Text = precio.ToString();
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
                txtPrecio.SelectAll();
                txtPrecio.Focus();
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
            if (e.Key == Key.Enter || e.Key == Key.Escape) return;

            TextBox textBox = sender as TextBox;
            if (e.Key == Key.OemComma && textBox.Text.Length == 0) e.Handled = true;

            if (e.Key == Key.Right || e.Key == Key.Left) e.Handled = true;

            if (!(Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) || (e.Key == Key.OemComma && textBox.Text.IndexOf(',') == -1) || e.Key == Key.Back))
            {
                e.Handled = true;
            }
            if (e.Key == Key.OemComma && textBox.Text.IndexOf(',') != -1) e.Handled = true;

            if (e.Key == Key.OemComma)
            {
                int commaIndex = textBox.Text.IndexOf(',');
                if (commaIndex != -1 && textBox.Text.Length - commaIndex > 2) e.Handled = true;
            }
            int indexOfComma = textBox.Text.IndexOf(',');
            if (indexOfComma != -1)
            {
                string decimalPart = textBox.Text.Substring(indexOfComma + 1);

                if (decimalPart.Length >= 2 && e.Key != Key.Back) e.Handled = true;
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
            txtPrecio.Text = precio.ToString();
        }
        private void Seleccionar()
        {
            DataRow nuevaFila = ventanaIndex.tablaVenta.NewRow();
            nuevaFila["idProducto"] = Id;
            nuevaFila["nombre_producto"] = nombre;
            nuevaFila["precio_compra"] = txtPrecio.Text;
            nuevaFila["Cantidad"] = cantidad;
            nuevaFila["Total"] = (cantidad * double.Parse(txtPrecio.Text));
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
