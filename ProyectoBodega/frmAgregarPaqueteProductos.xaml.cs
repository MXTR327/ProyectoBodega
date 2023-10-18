using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProyectoBodega
{
    public partial class frmAgregarPaqueteProductos : Window
    {
        internal VentanaProductos VentanaProductos;
        internal index VentanaIndex;

        CN_CargarLista cn_listaproveedor = new CN_CargarLista();
        //------------------------------------------------------------------------------------------------------------------------------\\
        public frmAgregarPaqueteProductos()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarlistaProveedor();
            CargarlistaCategoria();
            CargarlistaMarca();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void chkDescripcion_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcion.Text = "";
            txtDescripcion.IsReadOnly = !txtDescripcion.IsReadOnly;
            if (txtDescripcion.IsReadOnly == false)
            {
                gridDescripcion.Cursor = Cursors.IBeam;
                txtDescripcion.Focus();
            }
            else
            {
                gridDescripcion.Cursor = Cursors.Arrow;
                txtbDescripcion.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void CargarComboBox(ComboBox comboBox, DataTable dt, string valueMember, string displayMember, string defaultValue, int selectedIndex)
        {
            DataRow filaCero = dt.NewRow();
            filaCero[valueMember] = 0;
            filaCero[displayMember] = defaultValue;
            dt.Rows.InsertAt(filaCero, 0);

            comboBox.ItemsSource = dt.DefaultView;
            comboBox.SelectedValuePath = valueMember;
            comboBox.DisplayMemberPath = displayMember;

            if (selectedIndex >= 0 && selectedIndex < comboBox.Items.Count)
            {
                comboBox.SelectedIndex = selectedIndex;
            }
            else
            {
                comboBox.SelectedIndex = 0;
            }
        }
        private void CargarlistaProveedor()
        {
            DataTable dt = cn_listaproveedor.ListarProveedor();
            CargarComboBox(cmbProveedor, dt, "idProveedor", "nombre_proveedor", "Seleccionar proveedor", cmbProveedor.SelectedIndex);
        }
        private void CargarlistaCategoria()
        {
            DataTable dt = cn_listaproveedor.ListarCategoria();
            CargarComboBox(cmbCategoria, dt, "idCategoria", "nombre_categoria", "Seleccionar Categoria", cmbCategoria.SelectedIndex);
        }
        private void CargarlistaMarca()
        {
            DataTable dt = cn_listaproveedor.ListarMarca();
            CargarComboBox(cmbMarca, dt, "idMarca", "nombre_marca", "Seleccionar Marca", cmbMarca.SelectedIndex);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnSalir_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtStockPaquete_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal stockPaquetes = string.IsNullOrEmpty(txtPaqueteStock.Text) ? 0 : decimal.Parse(txtPaqueteStock.Text);
            decimal stockUnidad = string.IsNullOrEmpty(txtUnidadStock.Text) ? 0 : decimal.Parse(txtUnidadStock.Text);
            decimal precioCompraPaquete = string.IsNullOrEmpty(txtPaquetePrecioCompra.Text) ? 0 : decimal.Parse(txtPaquetePrecioCompra.Text);
            decimal precioVentaPaquete = string.IsNullOrEmpty(txtPaquetePrecioVenta.Text) ? 0 : decimal.Parse(txtPaquetePrecioVenta.Text);
            decimal precioVentaUnidad = string.IsNullOrEmpty(txtUnidadPrecioVenta.Text) ? 0 : decimal.Parse(txtUnidadPrecioVenta.Text);

            txtUnidadTotal.Text = (stockPaquetes * stockUnidad).ToString();

            txtPaqueteCompraTotal.Text = (stockPaquetes * precioCompraPaquete).ToString("F2");

            txtUnidadPrecioCompra.Text = precioCompraPaquete.ToString("F2");

            txtUnidadCompraTotal.Text = (stockUnidad * decimal.Parse(txtUnidadPrecioCompra.Text)).ToString("F2");
            txtPaqueteGananciaPaquete.Text = (precioVentaPaquete - precioCompraPaquete).ToString("F2");
            txtPaqueteGananciaTotal.Text = (decimal.Parse(txtPaqueteGananciaPaquete.Text) * stockPaquetes).ToString("F2");
            txtUnidadGananciaUnidad.Text = (precioVentaUnidad - decimal.Parse(txtUnidadPrecioCompra.Text)).ToString("F2");
            txtUnidadGananciaTotal.Text = (decimal.Parse(txtUnidadGananciaUnidad.Text) * decimal.Parse(txtUnidadTotal.Text)).ToString("F2");
        }


        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtStockPaquete_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                return;
            }
            if (!char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) && e.Key != Key.Back || (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                e.Handled = true;
            }
        }
        private void txtPaquetePrecioCompra_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                return;
            }
            if (!(Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) ||
                  (e.Key == Key.OemComma && textBox.Text.IndexOf(',') == -1) ||
                  e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Home || e.Key == Key.End))
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
        private void btnAgregarPaquete_Click(object sender, RoutedEventArgs e)
        {
            if (!validar())
            {
                return;
            }
            string nombreProducto = txtNombre.Text;
            string descripcion = txtDescripcion.Text;
            double precioCompraPack = double.Parse(txtPaquetePrecioCompra.Text);
            double precioVentaPack = double.Parse(txtPaquetePrecioVenta.Text);
            string medida = txtMedida.Text;
            int sPack = int.Parse(txtPaqueteStock.Text);
            string idCategoria = cmbCategoria.SelectedValue.ToString();
            string idProveedor = cmbProveedor.SelectedValue.ToString();
            string idMarca = cmbMarca.SelectedValue.ToString();
            double precioCompraUni = double.Parse(txtUnidadPrecioCompra.Text);
            double precioVentaUni = double.Parse(txtUnidadPrecioVenta.Text);
            int sUnidad = int.Parse(txtUnidadTotal.Text);

            CN_frmAgregarPaqueteProductos paquete = new CN_frmAgregarPaqueteProductos("PAQ " + nombreProducto, descripcion, precioCompraPack, precioVentaPack, medida, sPack, idCategoria, idProveedor, idMarca);
            CN_frmAgregarPaqueteProductos unidad = new CN_frmAgregarPaqueteProductos(nombreProducto, descripcion, precioCompraUni, precioVentaUni, medida, sUnidad, idCategoria, idProveedor, idMarca);
            if (!paquete.VerificarExistencia() || !unidad.VerificarExistencia())
            {
                MessageBox.Show("El nombre ya EXISTE, intente con otro nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool rptaP = paquete.SubirProducto();
            bool rptaU = unidad.SubirProducto();
            if (rptaP || rptaU)
            {
                ActualizarTablas();
                if (rptaP)
                    MessageBox.Show("El Paquete se subió correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                if (rptaU)
                    MessageBox.Show("Las unidades se subieron correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                txtNombre.Text = "";
                txtDescripcion.Text = "";
                txtPaquetePrecioCompra.Text = "";
                txtPaquetePrecioVenta.Text = "";
                txtMedida.Text = "";
                txtPaqueteStock.Text = "";
                txtUnidadStock.Text = "";
                cmbCategoria.SelectedValue = 0;
                cmbProveedor.SelectedValue = 0;
                cmbMarca.SelectedValue = 0;
                txtUnidadPrecioCompra.Text = "";
                txtUnidadPrecioVenta.Text = "";

                txtNombre.Focus();
                txtbMedida.Visibility = Visibility.Visible;
                txtbDescripcion.Visibility = Visibility.Visible;
                txtbPaqueteStock.Visibility = Visibility.Visible;
                txtbUnidadStock.Visibility = Visibility.Visible;
                txtbPaquetePrecioCompra.Visibility = Visibility.Visible;
                txtbPaquetePrecioVenta.Visibility = Visibility.Visible;
                txtbUnidadPrecioVenta.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Error al intentar subir el paquete o las unidades a la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ActualizarTablas()
        {
            if (VentanaProductos != null)
            {
                VentanaProductos.CargarProducto();
                if (VentanaProductos.dgProducto.Items.Count > 0)
                {
                    VentanaProductos.dgProducto.SelectedIndex = 0;
                }
            }
            if (VentanaIndex != null)
            {
                VentanaIndex.CargarProducto();
                if (VentanaIndex.dgProducto.Items.Count > 0)
                {
                    VentanaIndex.dgProducto.SelectedIndex = 0;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private bool validar()
        {
            bool rpta = true;
            List<TextBox> campos = new List<TextBox>
            {
                txtNombre,
                txtMedida,
                txtPaqueteStock,
                txtUnidadStock,
                txtPaquetePrecioCompra,
                txtPaquetePrecioVenta,
                txtUnidadPrecioVenta
            };
            foreach (TextBox textBox in campos)
            {
                string valor = textBox.Text;
                if (string.IsNullOrEmpty(valor))
                {
                    ResaltarCampoVacio(textBox);

                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(0.5);
                    timer.Tick += (sender, e) =>
                    {
                        RestaurarCampo(textBox);
                        timer.Stop();
                    };
                    timer.Start();

                    rpta = false;
                }
            }
            if (cmbProveedor.SelectedIndex == 0 || cmbCategoria.SelectedIndex == 0 || cmbMarca.SelectedIndex == 0)
            {
                MessageBox.Show("No eligió un Proveedor / Categoría / Marca", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                rpta = false;
            }
            return rpta;
        }
        private void ResaltarCampoVacio(TextBox campo)
        {
            campo.Background = Brushes.Red;
        }
        private void RestaurarCampo(TextBox campo)
        {
            campo.Background = Brushes.Transparent;
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
        private void txtNombre_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbNombre.Visibility=Visibility.Collapsed;
        }
        private void txtNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                txtbNombre.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtMedida_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbMedida.Visibility = Visibility.Collapsed;
        }
        private void txtMedida_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMedida.Text))
            {
                txtbMedida.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtDescripcion_GotFocus(object sender, RoutedEventArgs e)
        {
            if (chkDescripcion.IsChecked == true)
            {
                txtbDescripcion.Visibility = Visibility.Collapsed;
            }
        }
        private void txtDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                txtbDescripcion.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtPaqueteStock_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbPaqueteStock.Visibility = Visibility.Collapsed;
        }
        private void txtPaqueteStock_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPaqueteStock.Text))
            {
                txtbPaqueteStock.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtUnidadStock_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbUnidadStock.Visibility = Visibility.Collapsed;
        }
        private void txtUnidadStock_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUnidadStock.Text))
            {
                txtbUnidadStock.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtPaquetePrecioCompra_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbPaquetePrecioCompra.Visibility = Visibility.Collapsed;
        }
        private void txtPaquetePrecioCompra_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPaquetePrecioCompra.Text))
            {
                txtbPaquetePrecioCompra.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtPaquetePrecioVenta_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbPaquetePrecioVenta.Visibility = Visibility.Collapsed;
        }
        private void txtPaquetePrecioVenta_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPaquetePrecioVenta.Text))
            {
                txtbPaquetePrecioVenta.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtUnidadPrecioVenta_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbUnidadPrecioVenta.Visibility = Visibility.Collapsed;
        }
        private void txtUnidadPrecioVenta_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUnidadPrecioVenta.Text))
            {
                txtbUnidadPrecioVenta.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Close();
            }
            else if (e.Key == Key.Enter)
            {
                btnAgregarPaquete_Click(sender, e);
            }
        }
    }
}
