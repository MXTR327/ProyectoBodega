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
    public partial class frmAgregarProducto : Window
    {
        internal VentanaProductos VentanaProductos;
        internal index VentanaIndex;

        CN_CargarLista cn_listaproveedor = new CN_CargarLista();
        public frmAgregarProducto()
        {
            InitializeComponent();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarlistaProveedor();
            CargarlistaCategoria();
            CargarlistaMarca();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
        CN_frmAgregarProductos verificar = new CN_frmAgregarProductos();
        private void btnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (!validar())
            {
                txtNombre.Focus();
                return;
            }
            string nombre_producto = txtNombre.Text;
            string descripcion = txtDescripcion.Text;
            double precio_compra = double.Parse(txtPrecioCompra.Text);
            double precio_venta = double.Parse(txtPrecioVenta.Text);
            string medida = txtMedida.Text;
            int stock = int.Parse(txtStock.Text);
            string nombre_Categoria = cmbCategoria.SelectedValue.ToString();
            string nombre_Proveedor = cmbProveedor.SelectedValue.ToString();
            string nombre_Marca = cmbMarca.SelectedValue.ToString();

            if (verificar.VerificarExistencia(nombre_producto))
            {
                CN_frmAgregarProductos productos = new CN_frmAgregarProductos(nombre_producto, descripcion, precio_compra, precio_venta, medida, stock, nombre_Categoria, nombre_Proveedor, nombre_Marca);

                if (productos.SubirProducto())
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
                    }
                    MessageBox.Show("El producto se subió correctamente", "Éxito");
                    txtNombre.Text = "";
                    txtDescripcion.Text = "";
                    cmbProveedor.SelectedIndex = 0;
                    cmbCategoria.SelectedIndex = 0;
                    cmbMarca.SelectedIndex = 0;
                    txtStock.Text = "";
                    txtMedida.Text = "";
                    txtPrecioCompra.Text = "";
                    txtPrecioVenta.Text = "";
                    txtNombre.Focus();
                }
                else
                {
                    MessageBox.Show("Ocurrio un error al intentar subir a la Base de datos", "Error al Insertar");
                }
            }
            else
            {
                MessageBox.Show("El producto ya EXISTE", "Producto ya existente");
            }
        }
        private bool validar()
        {
            bool rpta = true;
            List<TextBox> campos = new List<TextBox>
            {
                txtNombre,
                txtStock,
                txtMedida,
                txtPrecioCompra,
                txtPrecioVenta
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
            campo.ClearValue(TextBox.BackgroundProperty);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnAgregarProducto_Click(sender, e);
            }
        }
        private void txtPrimeraLetraMayuscula_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                string newText = char.ToUpper(textBox.Text[0]) + textBox.Text.Substring(1);
                textBox.Text = newText;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
        private void txtCalcularGanancias_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtStock.Text, out int stock) &&
                double.TryParse(txtPrecioCompra.Text, out double precioCompra) &&
                double.TryParse(txtPrecioVenta.Text, out double precioVenta))
            {
                double gananciaUnidad = precioVenta - precioCompra;

                txtGananciaPorUnidad.Text = gananciaUnidad.ToString("F2");
                txtGananciaTotal.Text = (gananciaUnidad * stock).ToString("F2");
            }
            else
            {
                txtGananciaPorUnidad.Text = "00,00";
                txtGananciaTotal.Text = "00,00";
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
        private void txtControlarDouble_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

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
        private void txtNombre_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbNombre.Visibility = Visibility.Collapsed;
        }

        private void txtNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                txtbNombre.Visibility = Visibility.Visible;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtDescripcion_GotFocus(object sender, RoutedEventArgs e)
        {
            if(chkDescripcion.IsChecked == true)
            {
                txtbDescripcion.Visibility = Visibility.Collapsed;
            }
        }

        private void txtDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                txtbDescripcion.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtMedida_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbMedida.Visibility = Visibility.Collapsed;
        }
        private void txtMedida_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMedida.Text))
            {
                txtbMedida.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtStock_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbStock.Visibility = Visibility.Collapsed;
        }
        private void txtStock_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStock.Text))
            {
                txtbStock.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtPrecioCompra_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbPrecioCompra.Visibility = Visibility.Collapsed;
        }
        private void txtPrecioCompra_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPrecioCompra.Text))
            {
                txtbPrecioCompra.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtPrecioVenta_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbPrecioVenta.Visibility = Visibility.Collapsed;
        }
        private void txtPrecioVenta_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPrecioVenta.Text))
            {
                txtbPrecioVenta.Visibility = Visibility.Visible;
            }
        }
    }
}
