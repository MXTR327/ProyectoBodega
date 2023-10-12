using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Negocio;
using System.Data;
using System.Windows.Media;

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
            if(indexVentana != null)
            {
                txtTotal.Text = (string)indexVentana.lblTotal.Content;
            }
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void txtPago_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPago.Text) && !string.IsNullOrWhiteSpace(txtTotal.Text))
            {
                if (double.TryParse(txtPago.Text, out double pago) && double.TryParse(txtTotal.Text, out double total))
                {
                    txtCambio.Text = (pago - total).ToString("F2");
                    if( double.Parse(txtCambio.Text) < 0)
                    {
                        txtCambio.Foreground = Brushes.Red;
                    }
                    else
                    {
                        txtCambio.Foreground = Brushes.Green;
                    }
                }
                else
                {
                    txtCambio.Text = "-1";
                }
            }
            else
            {
                txtCambio.Text = "0,00";
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtControlarDouble_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (e.Key == Key.OemComma && textBox.Text.Length == 0)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Right || e.Key == Key.Left)
            {
                e.Handled = true;
            }
            if (!(Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) || (e.Key == Key.OemComma && textBox.Text.IndexOf(',') == -1) || e.Key == Key.Back))
            {
                e.Handled = true;
            }
            
            if (e.Key == Key.OemComma && textBox.Text.IndexOf(',') != -1)
            {
                e.Handled = true;
            }
            if (e.Key == Key.OemComma)
            {
                int commaIndex = textBox.Text.IndexOf(',');
                if (commaIndex != -1 && textBox.Text.Length - commaIndex > 2)
                {
                    e.Handled = true;
                }
            }
            int indexOfComma = textBox.Text.IndexOf(',');
            if (indexOfComma != -1)
            {
                string decimalPart = textBox.Text.Substring(indexOfComma + 1);
                if (decimalPart.Length >= 2 && e.Key != Key.Back)
                {
                    e.Handled = true;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtPrimeraLetraMayuscula_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                string newText = char.ToUpper(textBox.Text[0]) + textBox.Text.Substring(1).ToLower();
                textBox.Text = newText;
                textBox.SelectionStart = textBox.Text.Length;
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

            if (!double.TryParse(txtTotal.Text, out double total_venta))
            {
                MessageBox.Show("El valor en el campo 'Total' no es válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double ganancia = 0;

            if (!double.TryParse(txtCambio.Text, out double vuelto))
            {
                MessageBox.Show("El valor en el campo 'Cambio' no es válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CN_frmCobrar venta = new CN_frmCobrar(nombre_cliente, idVendedor, total_venta, ganancia, vuelto);
            int idVenta = venta.AgregarVenta();

            double gananciaTotalProductos = 0;

            if (indexVentana.dgVenta.Items.Count > 0)
            {
                foreach (var item in indexVentana.dgVenta.Items)
                {
                    DataRowView fila = (DataRowView)item;
                    int idProducto = Convert.ToInt32(fila["idProducto"]);
                    int cantidadProducto = Convert.ToInt32(fila["Cantidad"]);
                    double precioUnitarioProducto = Convert.ToDouble(fila["precio_compra"]);

                    CN_frmCobrar detalleVenta = new CN_frmCobrar(idVenta, idProducto, cantidadProducto, precioUnitarioProducto);
                    double gananciaProducto = detalleVenta.AgregarDetalleVenta();

                    gananciaTotalProductos += gananciaProducto;
                }
            }

            CN_frmCobrar actualizarVenta = new CN_frmCobrar(idVenta, gananciaTotalProductos);
            bool rpta = actualizarVenta.ActualizarGananciaVenta();

            MessageBox.Show("Venta realizada correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            indexVentana.CargarProducto();
            indexVentana.tablaVenta.Rows.Clear();
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
            if (string.IsNullOrEmpty(txtPago.Text))
            {
                txtbPago.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                txtNombre.Text = "Anónimo";
            }
        }
        private void txtNombre_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (txtNombre.Text == "Anónimo")
            {
                txtNombre.Text = "";
            }
        }
    }
}
