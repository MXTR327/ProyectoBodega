using Negocio;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoBodega
{
    public partial class ventanaVentas : Window
    {
        CN_ventanaVentas cn_ventanaventas = new CN_ventanaVentas();
        private string filtroFecha;

        public ventanaVentas()
        {
            InitializeComponent();
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            filtroFecha = "";
            CargarVentas();

            dpFecha.SelectedDate = DateTime.Now;

            dgVentas.SelectedIndex = 0;
            dgProductosVenta.SelectedIndex = 0;
        }
        private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is DatePicker datePicker)) return;

            DateTime? fechaSeleccionada = datePicker.SelectedDate;

            if (fechaSeleccionada.HasValue)
            {
                filtroFecha = fechaSeleccionada.Value.Date.ToString("dd/MM/yyyy");
                CargarVentas();
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void CalcularGananciasTotales()
        {
            decimal suma = 0;
            for (int i = 0; i < dgVentas.Items.Count; i++)
            {
                var fila = dgVentas.Items[i];

                if (fila is DataRowView dataRowView)
                {
                    if (decimal.TryParse(dataRowView["ganancia"]?.ToString(), out decimal ganancia))
                    {
                        suma += ganancia;
                    }
                }
            }
            lblGanancia.Content = suma.ToString("F2");
        }
        private void Total()
        {
            decimal suma = 0;
            for (int i = 0; i < dgVentas.Items.Count; i++)
            {
                var fila = dgVentas.Items[i];

                if (fila is DataRowView dataRowView)
                {
                    if (decimal.TryParse(dataRowView["Total_Venta"]?.ToString(), out decimal totalVenta))
                    {
                        suma += totalVenta;
                    }
                }
            }
            lblTotal.Content = suma.ToString("F2");
        }

        //------------------------------------------------------------------------------------------------------------------------------\\
        private void CargarVentas()
        {
            DataTable dt = cn_ventanaventas.tblVentas(filtroFecha);
            dgVentas.ItemsSource = dt.DefaultView;
            CalcularGananciasTotales();
            Total();
        }
        private void CargarDetalleVentas()
        {
            if (dgVentas.Items.Count <= 0)
            {
                dgProductosVenta.ItemsSource = null;
                txtID.Text = "Sin Productos";
                txtNombre.Text = txtDescripcion.Text = txtCategoria.Text = txtProveedor.Text = txtMarca.Text = txtPrecioCompra.Text =
                txtPrecioVenta.Text = txtMedida.Text = txtStock.Text = txtUnidadGanancia.Text = txtGananciaTotal.Text = "Inexistente";

                return;
            }

            DataRowView filaSeleccionada = (DataRowView)dgVentas.SelectedItem;
            if (filaSeleccionada != null)
            {
                string Id = filaSeleccionada["idVenta"].ToString();
                DataTable dt = cn_ventanaventas.tblDetalleVenta(Id);
                dgProductosVenta.ItemsSource = dt.DefaultView;
                dgProductosVenta.SelectedIndex = 0;
            }
        }
        private void CargarDetalleProductos()
        {
            if (dgProductosVenta.Items.Count <= 0) return;

            DataRowView filaSeleccionada = (DataRowView)dgProductosVenta.SelectedItem;
            if (filaSeleccionada == null) return;

            string IdProducto = filaSeleccionada["idProducto"].ToString();
            string cantidad = filaSeleccionada["Cantidad"].ToString();
            DataTable DetalleProducto = cn_ventanaventas.tblDetalleProducto(IdProducto);

            if (DetalleProducto.Rows.Count <= 0) return;

            DataRow Filaproducto = DetalleProducto.Rows[0];
            txtID.Text = IdProducto;
            txtNombre.Text = Filaproducto["nombre_producto"].ToString();
            txtDescripcion.Text = Filaproducto["descripcion"].ToString();
            txtCategoria.Text = Filaproducto["nombre_categoria"].ToString();
            txtProveedor.Text = Filaproducto["nombre_proveedor"].ToString();
            txtMarca.Text = Filaproducto["nombre_marca"].ToString();
            txtPrecioCompra.Text = Filaproducto["precio_compra"].ToString();
            txtPrecioVenta.Text = Filaproducto["precio_venta"].ToString();
            txtMedida.Text = Filaproducto["medida"].ToString();
            txtStock.Text = Filaproducto["stock"].ToString();
            double gananciaUnidad = double.Parse(txtPrecioVenta.Text) - double.Parse(txtPrecioCompra.Text);
            double gananciaTotal = gananciaUnidad * int.Parse(cantidad);
            txtUnidadGanancia.Text = gananciaUnidad.ToString();
            txtGananciaTotal.Text = gananciaTotal.ToString();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void dgVentas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarDetalleVentas();
        }
        private void dgProductosVenta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarDetalleProductos();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnFechaHoy_Click(object sender, RoutedEventArgs e)
        {
            dpFecha.SelectedDate = DateTime.Now;
            dgVentas.SelectedIndex = 0;
            dgProductosVenta.SelectedIndex = 0;
        }
        private void btnReestablecerFecha_Click(object sender, RoutedEventArgs e)
        {
            dpFecha.SelectedDate = null;
            filtroFecha = "";
            CargarVentas();
            dgVentas.SelectedIndex = 0;
            dgProductosVenta.SelectedIndex = 0;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape) Close();
        }
    }
}
