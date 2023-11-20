    using Negocio;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ComboBox = System.Windows.Controls.ComboBox;

namespace ProyectoBodega
{
    public partial class VentanaProductos : Window
    {
        CN_VentanaProductos cn_ventanaproductos = new CN_VentanaProductos();
        CN_CargarLista cn_listaproveedor = new CN_CargarLista();

        internal index ventanaIndex;

        private string filtro;
        private string nombreProductoOriginal;

        public VentanaProductos()
        {
            InitializeComponent();
        }
        private void btnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnAgregarProductoUnidad_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = false;
            frmAgregarProducto frmProducto = new frmAgregarProducto();
            frmProducto.VentanaProductos = this;
            frmProducto.VentanaIndex = ventanaIndex;
            frmProducto.ShowDialog();
            this.ShowInTaskbar = true;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            filtro = "";
            CargarProducto();
            CargarProveedor();
            CargarCategoria();
            CargarMarca();

            CargarlistaProveedor();
            CargarlistaCategoria();
            CargarlistaMarca();

            if (dgProducto.Items.Count > 0) dgProducto.SelectedIndex = 0;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        public void CargarMarca()
        {
            DataTable dt = cn_ventanaproductos.tblMarca(filtro);
            dgMarca.ItemsSource = dt.DefaultView;
        }
        public void CargarCategoria()
        {
            DataTable dt = cn_ventanaproductos.tblCategoria(filtro);
            dgCategoria.ItemsSource = dt.DefaultView;
        }
        public void CargarProveedor()
        {
            DataTable dt = cn_ventanaproductos.tblProveedor(filtro);
            dgProveedor.ItemsSource = dt.DefaultView;
        }
        public void CargarProducto()
        {
            DataTable dt = cn_ventanaproductos.tblProducto(filtro);
            dgProducto.ItemsSource = dt.DefaultView;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void CargarComboBox(ComboBox comboBox, DataTable dt, string columnaID, string columnaNombre, string primerValor, int selectedIndex)
        {
            DataRow filaCero = dt.NewRow();
            filaCero[columnaID] = 0;
            filaCero[columnaNombre] = primerValor;
            dt.Rows.InsertAt(filaCero, 0);

            comboBox.ItemsSource = dt.DefaultView;
            comboBox.SelectedValuePath = columnaID;
            comboBox.DisplayMemberPath = columnaNombre;

            comboBox.SelectedIndex = selectedIndex >= 0 && selectedIndex < comboBox.Items.Count ? selectedIndex : 0;
        }
        public void CargarlistaProveedor()
        {
            DataTable dt = cn_listaproveedor.ListarProveedor();
            CargarComboBox(cmbProveedor, dt, "idProveedor", "nombre_proveedor", "Seleccionar proveedor", cmbProveedor.SelectedIndex);
        }
        public void CargarlistaCategoria()
        {
            DataTable dt = cn_listaproveedor.ListarCategoria();
            CargarComboBox(cmbCategoria, dt, "idCategoria", "nombre_categoria", "Seleccionar Categoria", cmbCategoria.SelectedIndex);
        }
        public void CargarlistaMarca()
        {
            DataTable dt = cn_listaproveedor.ListarMarca();
            CargarComboBox(cmbMarca, dt, "idMarca", "nombre_marca", "Seleccionar Marca", cmbMarca.SelectedIndex);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtBuscadorProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPrimeraLetraMayuscula_TextChanged(sender,e);
            filtro = txtBuscadorProducto.Text;
            CargarProducto();
        }
        private void txtBuscarProveedor_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPrimeraLetraMayuscula_TextChanged(sender, e);
            filtro = txtBuscadorProveedor.Text;
            CargarProveedor();
        }
        private void txtBuscarCategoria_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPrimeraLetraMayuscula_TextChanged(sender, e);
            filtro = txtBuscadorCategoria.Text;
            CargarCategoria();
        }
        private void txtBuscarMarca_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPrimeraLetraMayuscula_TextChanged(sender, e);
            filtro = txtBuscadorMarca.Text;
            CargarMarca();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnAgregarCategoria_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = false;
            frmAgregarCategoria frmagregarcategoria = new frmAgregarCategoria();
            frmagregarcategoria.ventanaProducto = this;
            frmagregarcategoria.ShowDialog();
            this.ShowInTaskbar = true;
        }
        private void btnCrearProveedor_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = false;
            frmAgregarProveedor frmagregarproveedor = new frmAgregarProveedor();
            frmagregarproveedor.ventanaProducto = this;
            frmagregarproveedor.ShowDialog();
            this.ShowInTaskbar = true;
        }
        private void btnCrearMarca_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = false;
            frmAgregarMarca frmagregarmarca = new frmAgregarMarca();
            frmagregarmarca.ventanaProducto = this;
            frmagregarmarca.ShowDialog();
            this.ShowInTaskbar = true;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnBorrarProveedor_Click(object sender, RoutedEventArgs e)
        {
            BorrarProveedor();
        }
        private void BorrarProveedor()
        {
            DataRowView filaSeleccionada = (DataRowView)dgProveedor.SelectedItem;

            if (filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una fila a borrar", "Error");
                return;
            }
            string Id = filaSeleccionada["idProveedor"].ToString();
            string nombre = filaSeleccionada["nombre_proveedor"].ToString();

            MessageBoxResult result = MessageBox.Show($"Esta a punto de borrar el siguiente Proveedor:\nID: {Id} \nNombre: {nombre}", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result != MessageBoxResult.Yes) return;

            cn_ventanaproductos.idProveedor = Id;
            if (!cn_ventanaproductos.borrarProveedor())
            {
                MessageBox.Show("Ocurrió un error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CargarProveedor();
            CargarlistaProveedor();
            if (dgProveedor.Items.Count > 0) dgProveedor.SelectedIndex = 0;
        }
        private void btnBorrarCategoria_Click(object sender, RoutedEventArgs e)
        {
            BorrarCategoria();
        }
        private void BorrarCategoria()
        {
            DataRowView filaSeleccionada = (DataRowView)dgCategoria.SelectedItem;

            if (filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una fila a borrar", "Error");
                return;
            }

            string Id = filaSeleccionada["idCategoria"].ToString();
            string nombre = filaSeleccionada["nombre_categoria"].ToString();
            MessageBoxResult result = MessageBox.Show($"Esta a punto de borrar la siguiente Categoria:\nID: {Id} \nNombre: {nombre}", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result != MessageBoxResult.Yes) return;
            
            cn_ventanaproductos.idCategoria = Id;

            if (!cn_ventanaproductos.borrarCategoria())
            {
                MessageBox.Show("Ocurrió un error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CargarCategoria();
            CargarlistaCategoria();
            CargarProducto();

            if (dgCategoria.Items.Count > 0) dgCategoria.SelectedIndex = 0;
        }
        private void btnBorrarMarca_Click(object sender, RoutedEventArgs e)
        {
            BorrarMarca();
        }
        private void BorrarMarca()
        {
            DataRowView filaSeleccionada = (DataRowView)dgMarca.SelectedItem;

            if (filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una fila a borrar", "Error");
                return;
            }

            string Id = filaSeleccionada["idMarca"].ToString();
            string nombre = filaSeleccionada["nombre_marca"].ToString();

            MessageBoxResult result = MessageBox.Show($"Esta a punto de borrar la siguiente Marca:\nID: {Id} \nNombre: {nombre}", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result != MessageBoxResult.Yes) return;
            cn_ventanaproductos.idMarca = Id;

            if (!cn_ventanaproductos.borrarMarca()) return;

            CargarMarca();
            CargarlistaMarca();

            if (dgMarca.Items.Count > 0) dgMarca.SelectedIndex = 0;
        }
        private void btnBorrarProducto_Click(object sender, RoutedEventArgs e)
        {
            BorrarProducto();
        }
        private void BorrarProducto()
        {
            DataRowView filaSeleccionada = (DataRowView)dgProducto.SelectedItem;

            if (filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una fila a borrar", "Error");
                return;
            }

            string Id = filaSeleccionada["idProducto"].ToString();
            string nombre = filaSeleccionada["nombre_producto"].ToString();

            MessageBoxResult result = MessageBox.Show($"Esta a punto de borrar el siguiente Producto:\nID: {Id} \nNombre: {nombre}", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            cn_ventanaproductos.idProducto = Id;

            if (!cn_ventanaproductos.borrarProducto())
            {
                MessageBox.Show("Ocurrió un error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CargarProducto();
            if (ventanaIndex != null)
            {
                ventanaIndex.CargarProducto();
            }
            if (dgProducto.Items.Count > 0) dgProducto.SelectedIndex = 0;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnActualizaProveedor_Click(object sender, RoutedEventArgs e)
        {
            DataRowView filaSeleccionada = (DataRowView)dgProveedor.SelectedItem;

            if (filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una fila de los proveedores a actualizar", "Error");
                return;
            }
            this.ShowInTaskbar = false;
            frmAgregarProveedor frmagregarproveedor = new frmAgregarProveedor();
            frmagregarproveedor.Tag = "Actualizar";
            frmagregarproveedor.ventanaProducto = this;
            frmagregarproveedor.ShowDialog();
            this.ShowInTaskbar = true;
        }
        private void btnActualizarCategoria_Click(object sender, RoutedEventArgs e)
        {
            DataRowView filaSeleccionada = (DataRowView)dgCategoria.SelectedItem;

            if (filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una fila de la categoria a actualizar", "Error");
                return;
            }
            this.ShowInTaskbar = false;
            frmAgregarCategoria frmagregarcategoria = new frmAgregarCategoria();
            frmagregarcategoria.Tag = "Actualizar";
            frmagregarcategoria.ventanaProducto = this;
            frmagregarcategoria.ShowDialog();
            this.ShowInTaskbar = true;
        }

        private void btnActualizarMarca_Click(object sender, RoutedEventArgs e)
        {
            DataRowView filaSeleccionada = (DataRowView)dgMarca.SelectedItem;

            if (filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una fila de la marca a actualizar", "Error");
                return;
            }
            this.ShowInTaskbar = false;
            frmAgregarMarca frmagregarmarca = new frmAgregarMarca();
            frmagregarmarca.Tag = "Actualizar";
            frmagregarmarca.ventanaProducto = this;
            frmagregarmarca.ShowDialog();
            this.ShowInTaskbar = true;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void dgProducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView filaSeleccionada = dgProducto.SelectedItem as DataRowView;

            if (filaSeleccionada == null || !filaSeleccionada.Row.Table.Columns.Contains("idProducto")) return;
            object idValue = filaSeleccionada["idProducto"];
            string idProducto = idValue.ToString();
            DataTable dt = new DataTable();
            dt = cn_ventanaproductos.ObtenerDatosProducto(idProducto);
            txtID.Text = idProducto;
            txtNombre.Text = dt.Rows[0][1].ToString();
            nombreProductoOriginal = dt.Rows[0][1].ToString();
            txtDescripcion.Text = dt.Rows[0][2].ToString();

            int idCategoria = Convert.ToInt32(dt.Rows[0][7]);
            int idProveedor = Convert.ToInt32(dt.Rows[0][8]);
            int idMarca = Convert.ToInt32(dt.Rows[0][9]);
            decimal precioCompra = Convert.ToDecimal(dt.Rows[0][3]);
            decimal precioVenta = Convert.ToDecimal(dt.Rows[0][4]);
            int Stock = Convert.ToInt32(dt.Rows[0][6]);
            cmbCategoria.SelectedValue = cmbCategoria.Items.Cast<DataRowView>().Any(item => Convert.ToInt32(item.Row[0]) == idCategoria) ? idCategoria : 0;
            cmbProveedor.SelectedValue = cmbProveedor.Items.Cast<DataRowView>().Any(item => Convert.ToInt32(item.Row[0]) == idProveedor) ? idProveedor : 0;
            cmbMarca.SelectedValue = cmbMarca.Items.Cast<DataRowView>().Any(item => Convert.ToInt32(item.Row[0]) == idMarca) ? idMarca : 0;

            txtPrecioCompra.Text = precioCompra.ToString("F2", CultureInfo.InvariantCulture);
            txtPrecioVenta.Text = precioVenta.ToString("F2", CultureInfo.InvariantCulture);
            txtMedida.Text = dt.Rows[0][5].ToString();
            txtStock.Text = Stock.ToString();

            decimal gananciaUnidad = precioVenta - precioCompra;
            txtGananciaUnidad.Text = gananciaUnidad.ToString("F2", CultureInfo.InvariantCulture);
            txtGananciaTotal.Text = (gananciaUnidad * Stock).ToString("F2", CultureInfo.InvariantCulture);
        }
        private void btnActualizarProducto_Click(object sender, RoutedEventArgs e)
        {
            DataRowView filaSeleccionada = (DataRowView)dgProducto.SelectedItem;

            if (filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una fila a Actualizar", "Error");
                return;
            }

            string Id = filaSeleccionada["idProducto"].ToString();
            string nombre = filaSeleccionada["nombre_producto"].ToString();
            MessageBoxResult result = MessageBox.Show($"¿Seguro que desea actualizar el siguiente Producto?:\nID: {Id} \nNombre: {nombre}", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            string idProducto = txtID.Text;
            string nombreProducto = txtNombre.Text;
            string descripcion = txtDescripcion.Text;

            decimal precioCompra,precioVenta;
            if (!decimal.TryParse(txtPrecioCompra.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out precioCompra) ||
                !decimal.TryParse(txtPrecioVenta.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out precioVenta))
            {
                MessageBox.Show("Ingrese valores válidos para precio compra y precio venta.", "Error en el formato");
                return;
            }
            string medida = txtMedida.Text;
            string stock = txtStock.Text;

            string nombreCategoria = cmbCategoria.SelectedValue.ToString();
            string nombreProveedor = cmbProveedor.SelectedValue.ToString();
            string nombreMarca = cmbMarca.SelectedValue.ToString();

            CN_VentanaProductos actualizar = new CN_VentanaProductos(idProducto, nombreProducto, descripcion, precioCompra, precioVenta, medida, stock, nombreCategoria, nombreProveedor, nombreMarca);

            if (nombreProductoOriginal != nombreProducto && !cn_ventanaproductos.verificarExistencia(nombreProducto))
            {
                MessageBox.Show("El producto ya existe puebre otro nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombre.Focus();
                return;
            }
            if (!actualizar.ActualizarProducto())
            {
                MessageBox.Show("Error al intentar Actualizar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int indiceFila = dgProducto.Items.IndexOf(dgProducto.SelectedItem);

            CargarProducto();

            if (ventanaIndex != null)
            {
                ventanaIndex.CargarProducto();

                if (ventanaIndex.dgProducto.Items.Count > 0) ventanaIndex.dgProducto.SelectedIndex = indiceFila;
            }

            if (dgProducto.Items.Count > 0) dgProducto.SelectedIndex = indiceFila;

            MessageBox.Show("El producto se Actualizó correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            nombreProductoOriginal = nombreProducto;
            txtNombre.Focus();

        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtPrecioCompra_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtStock.Text, out int stock) &&
                decimal.TryParse(txtPrecioCompra.Text,NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precioCompra) &&
                decimal.TryParse(txtPrecioVenta.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precioVenta))
            {
                decimal gananciaUnidad = precioVenta - precioCompra;
                txtGananciaUnidad.Text = gananciaUnidad.ToString("F2", CultureInfo.InvariantCulture);
                txtGananciaTotal.Text = (gananciaUnidad * stock).ToString("F2", CultureInfo.InvariantCulture);
            }
            else
            {
                txtGananciaUnidad.Text = "0.00";
                txtGananciaTotal.Text = "0.00";
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtStock_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) && e.Key != Key.Back || (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                e.Handled = true;
            }
        }
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
        private void txtBuscadorProducto_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbBuscar.Visibility = Visibility.Collapsed;
        }

        private void txtBuscadorProducto_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscadorProducto.Text)) txtbBuscar.Visibility = Visibility.Visible;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtBuscadorProveedor_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbBuscarProveedor.Visibility=Visibility.Collapsed;
        }

        private void txtBuscadorProveedor_LostFocus(object sender, RoutedEventArgs e)
        {   
            if (string.IsNullOrEmpty(txtBuscadorProveedor.Text)) txtbBuscarProveedor.Visibility = Visibility.Visible;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtBuscadorCategoria_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbBuscarCategoria.Visibility=Visibility.Collapsed;
        }

        private void txtBuscadorCategoria_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscadorCategoria.Text)) txtbBuscarCategoria.Visibility = Visibility.Visible;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtBuscadorMarca_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbBuscarMarca.Visibility=Visibility.Collapsed;
        }

        private void txtBuscadorMarca_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscadorMarca.Text)) txtbBuscarMarca.Visibility = Visibility.Visible;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape) Close();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void VerCategorias_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = false;
            BuscarCombobox buscar = new BuscarCombobox();
            buscar.Tag = "categoria";
            buscar.VentanaProductos = this;
            buscar.ShowDialog();
            this.ShowInTaskbar = true;
        }
        private void VerProveedor_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = false;
            BuscarCombobox buscar = new BuscarCombobox();
            buscar.Tag = "proveedor";
            buscar.VentanaProductos = this;
            buscar.ShowDialog();
            this.ShowInTaskbar = true;
        }
        private void VerMarca_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = false;
            BuscarCombobox buscar = new BuscarCombobox();
            buscar.Tag = "marca";
            buscar.VentanaProductos = this;
            buscar.ShowDialog();
            this.ShowInTaskbar = true;
        }
    }
}

