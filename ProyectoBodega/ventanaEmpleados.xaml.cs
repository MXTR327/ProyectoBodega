using Negocio;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProyectoBodega
{
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
            if (ventanaIndex != null) idActual = ventanaIndex.idvendedor;
            CargarVendedores();

            if (dgVendedores.Items.Count >0) dgVendedores.SelectedIndex = 0;
        }
        public void CargarVendedores()
        {
            DataTable dt = cn_ventanaempleados.tblEmpleados(filtro);
            dgVendedores.ItemsSource = dt.DefaultView;
        }
        
        private void txtBuscadorVendedor_TextChanged(object sender, TextChangedEventArgs e)
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

            filtro = txtBuscadorVendedor.Text;
            CargarVendedores();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnAgregarVendedor_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = false;
            frmAgregarVendedor agregarvendedor = new frmAgregarVendedor();
            agregarvendedor.ventanaEmpleados = this;
            agregarvendedor.Tag = "Crear";
            agregarvendedor.ShowDialog();
            this.ShowInTaskbar = true;
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
            this.ShowInTaskbar = false;
            var CuadroDialogo = new CuadroDialogo();
            CuadroDialogo.titulo = "Borrar Vendedor";

            if (CuadroDialogo.ShowDialog() != true) 
            {
                this.ShowInTaskbar = true;
                return; 
            }
            string valorIngresado = CuadroDialogo.ValorIngresado;

            if (valorIngresado == null || valorIngresado != filaSeleccionada["usuario"].ToString())
            {
                this.ShowInTaskbar = true;
                MessageBox.Show("Usuario Incorrecto", "Error");
                return;
            }

            string Id = filaSeleccionada["idVendedor"].ToString();
            string nombre = filaSeleccionada["nombre_vendedor"].ToString();

            MessageBoxResult result = MessageBox.Show($"Esta a punto de borrar el siguiente Vendedor:\nID: {Id} \nNombre: {nombre}", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

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
            this.ShowInTaskbar = true;
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
            this.ShowInTaskbar = false;
            var CuadroDialogo = new CuadroDialogo();
            CuadroDialogo.titulo = "Actualizar Vendedor";

            if (CuadroDialogo.ShowDialog() != true) 
            {
                this.ShowInTaskbar = true;
                return; 
            }

            string valorIngresado = CuadroDialogo.ValorIngresado;
            if (valorIngresado == null || valorIngresado != filaSeleccionada["usuario"].ToString())
            {
                this.ShowInTaskbar = true;
                MessageBox.Show("Usuario Incorrecto", "Error");
                return;
            }
            frmAgregarVendedor agregarvendedor = new frmAgregarVendedor();
            agregarvendedor.ventanaEmpleados = this;
            agregarvendedor.Tag = "Actualizar";
            agregarvendedor.ShowDialog();
            this.ShowInTaskbar = true;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtBuscadorVendedor_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbBuscador.Visibility=Visibility.Collapsed;
        }

        private void txtBuscadorVendedor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscadorVendedor.Text)) txtbBuscador.Visibility = Visibility.Visible;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void dgVendedores_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            btnActualizarVendedor_Click(sender,e);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            else if (e.Key == Key.F1) btnAgregarVendedor_Click(sender, e);
            else if (e.Key == Key.F2) btnActualizarVendedor_Click(sender, e);
            else if (e.Key == Key.F3) btnBorrarVendedor_Click(sender,e);
        }
    }
}
