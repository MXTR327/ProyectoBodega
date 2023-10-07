using Negocio;
using System.Data;
using System.Runtime.Remoting.Channels;
using System.Windows;

namespace ProyectoBodega
{
    /// <summary>
    /// Lógica de interacción para ventanaEmpleados.xaml
    /// </summary>
    public partial class ventanaEmpleados : Window
    {
        CN_ventanaEmpleados cn_ventanaempleados = new CN_ventanaEmpleados();
        private string filtro;
        internal index ventanaIndex;

        internal string idActual;

        public ventanaEmpleados()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            filtro = "";
            idActual = ventanaIndex.idvendedor;
            CargarVendedores();
        }
        public void CargarVendedores()
        {
            DataTable dt = cn_ventanaempleados.tblEmpleados(filtro);
            dgVendedores.ItemsSource = dt.DefaultView;
        }
        
        private void txtBuscadorVendedor_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            filtro = txtBuscadorProducto.Text;
            CargarVendedores();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnAgregarVendedor_Click(object sender, RoutedEventArgs e)
        {
            frmAgregarVendedor agregarvendedor = new frmAgregarVendedor();
            agregarvendedor.ventanaEmpleados = this;
            agregarvendedor.Tag = "Crear";
            agregarvendedor.ShowDialog();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnBorrarVendedor_Click(object sender, RoutedEventArgs e)
        {
            BorrarVendedor();
        }
        private void BorrarVendedor()
        {
            DataRowView filaSeleccionada = (DataRowView)dgVendedores.SelectedItem;
            if (filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una fila a borrar", "Error");
                return;
            }

            var CuadroDialogo = new CuadroDialogo();
            if (CuadroDialogo.ShowDialog() != true)
            {
                return;
            }
            string valorIngresado = CuadroDialogo.ValorIngresado;
            if (valorIngresado == null || valorIngresado != filaSeleccionada["usuario"].ToString())
            {
                MessageBox.Show("Usuario Incorrecto", "Error");
                return;
            }

            string Id = filaSeleccionada["idVendedor"].ToString();
            string nombre = filaSeleccionada["nombre_vendedor"].ToString();

            MessageBoxResult result = MessageBox.Show($"Esta a punto de borrar el siguiente Vendedor:\nID: {Id} \nNombre: {nombre}", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            cn_ventanaempleados.idVendedor = Id;
            if (!cn_ventanaempleados.borrarVendedor())
            {
                MessageBox.Show("Ocurrió un error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (idActual == Id)
            {
                Close();
                ventanaIndex.btnCerrarSesion_Click(ventanaIndex.btnCerrarSesion, null);
            }
            CargarVendedores();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnActualizarVendedor_Click(object sender, RoutedEventArgs e)
        {
            DataRowView filaSeleccionada = dgVendedores.SelectedItem as DataRowView;

            if (filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una fila a actualizar", "Error");
                return;
            }
            var CuadroDialogo = new CuadroDialogo();
            if (CuadroDialogo.ShowDialog() != true)
            {
                return;
            }
            string valorIngresado = CuadroDialogo.ValorIngresado;
            if (valorIngresado == null || valorIngresado != filaSeleccionada["usuario"].ToString())
            {
                MessageBox.Show("Usuario Incorrecto", "Error");
                return;
            }

            frmAgregarVendedor agregarvendedor = new frmAgregarVendedor();
            agregarvendedor.ventanaEmpleados = this;
            agregarvendedor.Tag = "A";
            agregarvendedor.ShowDialog();
        }
    }
}
