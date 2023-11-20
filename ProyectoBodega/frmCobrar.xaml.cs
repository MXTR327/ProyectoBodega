using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Negocio;
using System.Data;
using System.Windows.Media;
using System.Globalization;

namespace ProyectoBodega
{
    public partial class frmCobrar : Window
    {
        internal index indexVentana;

        public frmCobrar()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(indexVentana != null) txtTotal.Text = (string)indexVentana.lblTotal.Content;
            txtbPago.Focus();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void txtPago_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPago.Text) || string.IsNullOrWhiteSpace(txtTotal.Text))
            {
                txtCambio.Text = "0.00";
                return;
            }
            if (decimal.TryParse(txtPago.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal pago)
                && decimal.TryParse(txtTotal.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal total))
            {
                decimal cambio = pago - total;
                txtCambio.Text = cambio.ToString("F2", CultureInfo.InvariantCulture);
                txtCambio.Foreground = cambio < 0 ? Brushes.Red : Brushes.Green;
            }
            else
            {
                txtCambio.Text = "-1";
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtControlarDouble_PreviewKeyDown(object sender, KeyEventArgs e)
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
        private void txtPrimeraLetraMayuscula_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                int cursorPosition = textBox.SelectionStart;
                string nuevoTexto = char.ToUpper(textBox.Text[0]) + textBox.Text.Substring(1).ToLower();
                textBox.Text = nuevoTexto;
                textBox.SelectionStart = Math.Min(cursorPosition, textBox.Text.Length);
                textBox.SelectionLength = 0;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPago.Text))
            {
                MessageBox.Show("Agregue un pago", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPago.Focus();
            }
            else if (!(double.Parse(txtCambio.Text) >= 0))
            {
                MessageBox.Show("No hay suficiente dinero", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPago.Focus();
            }
            else
            {
                Seleccionar();
            }
        }

    private void Seleccionar()
        {
            string nombre_cliente = txtNombre.Text;
            int idVendedor = Convert.ToInt32(indexVentana.idvendedor);

            if (!decimal.TryParse(txtTotal.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal total_venta))
            {
                MessageBox.Show("El valor en el campo 'Total' no es válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal ganancia = 0;

            if (!decimal.TryParse(txtCambio.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal vuelto))
            {
                MessageBox.Show("El valor en el campo 'Cambio' no es válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CN_frmCobrar venta = new CN_frmCobrar(nombre_cliente, idVendedor, total_venta, ganancia, vuelto);
            int idVenta = venta.AgregarVenta();

            decimal gananciaTotalProductos = 0;

            if (indexVentana.dgVenta.Items.Count > 0)
            {
                foreach (var item in indexVentana.dgVenta.Items)
                {
                    DataRowView fila = (DataRowView)item;
                    int idProducto = Convert.ToInt32(fila["idProducto"]);
                    int cantidadProducto = Convert.ToInt32(fila["Cantidad"]);
                    decimal precioUnitarioProducto = Convert.ToDecimal(fila["precio_compra"], CultureInfo.InvariantCulture);

                    CN_frmCobrar detalleVenta = new CN_frmCobrar(idVenta, idProducto, cantidadProducto, precioUnitarioProducto);
                    decimal gananciaProducto = detalleVenta.AgregarDetalleVenta();

                    gananciaTotalProductos += gananciaProducto;
                }
            }

            CN_frmCobrar actualizarVenta = new CN_frmCobrar(idVenta, gananciaTotalProductos);
            bool rpta = actualizarVenta.ActualizarGananciaVenta();

            MessageBox.Show("Venta realizada correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            indexVentana.CargarProducto();
            indexVentana.tablaVenta.Rows.Clear();
            indexVentana.CalcularSumaTotal();
            Close();
        }
    //------------------------------------------------------------------------------------------------------------------------------\\
    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Ventana.Cursor = Cursors.SizeAll;
                DragMove();
                Ventana.Cursor = Cursors.Arrow;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtPago_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbPago.Visibility = Visibility.Collapsed;
        }

        private void txtPago_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPago.Text)) txtbPago.Visibility = Visibility.Visible;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text)) txtNombre.Text = "Anónimo";
        }
        private void txtNombre_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (txtNombre.Text == "Anónimo") txtNombre.Text = "";
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            else if (e.Key == Key.Enter) btnAceptar_Click(sender, e);
        }
    }
}
