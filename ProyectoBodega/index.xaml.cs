using Negocio;
using Presentacion;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoBodega
{
    public partial class index : Window
    {
        CN_index cn_index = new CN_index();

        internal string idvendedor;
        internal string nombrevendedor;
        private string filtroProductos;

        public DataTable tablaVenta;
        public index()
        {

            InitializeComponent();

            CrearDataTable();
            dgVenta.ItemsSource = tablaVenta.DefaultView;
        }
        private void CrearDataTable()
        {
            tablaVenta = new DataTable();

            tablaVenta.Columns.Add("Cuenta", typeof(string));
            tablaVenta.Columns.Add("idProducto", typeof(string));
            tablaVenta.Columns.Add("nombre_producto", typeof(string));
            tablaVenta.Columns.Add("precio_compra", typeof(string));
            tablaVenta.Columns.Add("Cantidad", typeof(string));
            tablaVenta.Columns.Add("Total", typeof(string));
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ventanaIndex_Loaded(object sender, RoutedEventArgs e)
        {
            txtIdVendedor.Text = idvendedor;
            txtNombreVendedor.Text = nombrevendedor;
            filtroProductos = "";
            CargarProducto();

            if (dgProducto.Items.Count > 0)
            {
                dgProducto.SelectedIndex = 0;
            }
        }
        public void CargarProducto()
        {
            DataTable dt = cn_index.tblProducto(filtroProductos);
            dgProducto.ItemsSource = dt.DefaultView;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        public void CalcularSumaTotal()
        {
            double suma = 0;

            for (int i = 0; i < dgVenta.Items.Count; i++)
            {
                var fila = dgVenta.Items[i];

                if (fila is DataRowView dataRowView)
                {
                    suma += Convert.ToDouble(dataRowView["total"]?.ToString() ?? "0");
                }
                else
                {
                }
            }

            lblTotal.Content = suma.ToString("F2");
        }
        public void ContadorProductosVenta()
        {
            for (int i = 0; i < dgVenta.Items.Count; i++)
            {
                if (dgVenta.Items[i] is DataRowView)
                {
                    var fila = (DataRowView)dgVenta.Items[i];
                    fila["Cuenta"] = (i + 1).ToString();
                }
                else
                {
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtBuscadorProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                string newText = char.ToUpper(textBox.Text[0]) + textBox.Text.Substring(1);
                textBox.Text = newText;
                textBox.SelectionStart = textBox.Text.Length;
            }

            filtroProductos = txtBuscadorProducto.Text;
            CargarProducto();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnInventario_Click(object sender, RoutedEventArgs e)
        {
            VentanaProductos ventanaProducto = new VentanaProductos();
            ventanaProducto.ventanaIndex = this;
            ventanaProducto.ShowDialog();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView filaSeleccionada = (DataRowView)dgProducto.SelectedItem;
            if (filaSeleccionada != null)
            {
                string ID = filaSeleccionada["idProducto"].ToString();

                foreach (DataRow fila in tablaVenta.Rows)
                {
                    string idProducto = fila["idProducto"].ToString();

                    if (idProducto == ID)
                    {
                        MessageBox.Show($"El producto ya fue agregado Elija otro producto","Producto ya existente");
                        return;
                    }
                }
                frmDetalleProducto detalle = new frmDetalleProducto();
                detalle.ventanaIndex = this;
                detalle.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione una fila", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        public void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            idvendedor = "0";
            nombrevendedor = "0";
            frmLogin1 ventanaLogin = new frmLogin1();
            ((App)Application.Current).ChangeMainWindow(ventanaLogin);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnVendedores_Click(object sender, RoutedEventArgs e)
        {
            ventanaEmpleados ventanaempleados = new ventanaEmpleados();
            ventanaempleados.ventanaIndex = this;
            ventanaempleados.ShowDialog();
        }
        private void btnVentas_Click(object sender, RoutedEventArgs e)
        {
            ventanaVentas ventanaventas = new ventanaVentas();

            ventanaventas.ShowDialog();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnCancelarVenta_Click(object sender, RoutedEventArgs e)
        {
            if(dgVenta.Items.Count < 0)
            {
                MessageBoxResult result = MessageBox.Show($"¿Esta seguro de que desea borrar todos los productos de la venta?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {   
                    tablaVenta.Rows.Clear();
                    CalcularSumaTotal();
                }
            }
            else
            {
                MessageBox.Show("No hay productos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void brnBorrarFila_Click(object sender, RoutedEventArgs e)
        {
            DataRowView filaSeleccionada = (DataRowView)dgVenta.SelectedItem;
            if (filaSeleccionada != null)
            {
                if (tablaVenta.Rows.Count > 0)
                {
                    int indicefila = dgVenta.SelectedIndex;
                    tablaVenta.Rows[indicefila].Delete();
                    tablaVenta.AcceptChanges();
                    CalcularSumaTotal();
                }
            }
            else
            {
                MessageBox.Show("Seleccione la fila a borrar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCobrarVenta_Click(object sender, RoutedEventArgs e)
        {
            if (dgVenta.Items.Count > 0)
            {
                frmCobrar frmcobrar = new frmCobrar();
                frmcobrar.indexVentana = this;
                frmcobrar.ShowDialog();
            }
            else
            {
                MessageBox.Show("Agregue productos para poder vender", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnBuscarCodigo_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscarPorCodigo.Text))
            {
                MessageBox.Show("Escriba un codigo antes de continuar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DataGridRow indiceFila = null;
            foreach (var item in dgProducto.Items)
            {
                DataGridRow row = (DataGridRow)dgProducto.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null)
                {
                    DataRowView dataRow = (DataRowView)row.Item;
                    if (dataRow["idProducto"] != null && dataRow["idProducto"].ToString() == txtBuscarPorCodigo.Text)
                    {
                        indiceFila = row;
                        break;
                    }
                }
            }
            if (indiceFila != null)
            {
                dgProducto.SelectedItem = indiceFila.Item;
                dgProducto.ScrollIntoView(indiceFila.Item);

                frmDetalleProducto indexVender = new frmDetalleProducto();
                indexVender.ventanaIndex = this;
                indexVender.ShowDialog();
            }
            else
            {
                MessageBox.Show("Código no encontrado o inexistente intente con otro", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnBuscarNombre_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscarPorNombre.Text))
            {
                MessageBox.Show("Escriba un nombre antes de continuar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string buscarTexto = txtBuscarPorNombre.Text;
            DataGridRow mejorCoincidencia = null;
            int mejorCoincidenciaIndice = -1;

            for (int i = 0; i < dgProducto.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dgProducto.ItemContainerGenerator.ContainerFromIndex(i);
                if (row != null)
                {
                    DataRowView dataRow = (DataRowView)row.Item;
                    if (dataRow["nombre_producto"] != null)
                    {
                        string nombreProducto = dataRow["nombre_producto"].ToString();
                        if (nombreProducto.Contains(buscarTexto) && nombreProducto.IndexOf(buscarTexto) == 0)
                        {
                            mejorCoincidencia = row;
                            mejorCoincidenciaIndice = i;
                            break;
                        }
                    }
                }
            }
            if (mejorCoincidencia != null)
            {
                dgProducto.SelectedItem = mejorCoincidencia.Item;
                dgProducto.ScrollIntoView(mejorCoincidencia.Item);

                frmDetalleProducto indexVender = new frmDetalleProducto();
                indexVender.ventanaIndex = this;
                indexVender.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nombre de producto no encontrado o inexistente. Intente con otro.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
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
        private void txtCantidad_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                return;
            }
            if (!char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) && e.Key != Key.Back  || (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                e.Handled = true;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void dgProducto_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnSeleccionar_Click(sender, e);
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
        private void txtBuscarPorCodigo_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbID.Visibility = Visibility.Collapsed;
        }
        private void txtBuscarPorCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscarPorCodigo.Text))
            {
                txtbID.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtBuscarPorNombre_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbNombre.Visibility = Visibility.Collapsed;
        }
        private void txtBuscarPorNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscarPorNombre.Text))
            {
                txtbNombre.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtBuscarPorCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnBuscarCodigo_Click(btnBuscarCodigo, e);
            }
        }
        private void txtBuscarPorNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnBuscarNombre_Click(btnBuscarCodigo, e);
            }
        }
    }
}
