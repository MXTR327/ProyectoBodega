using Negocio;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace ProyectoBodega
{
    public partial class frmAgregarMarca : Window
    {
        internal VentanaProductos ventanaProducto;
        CN_frmAgregarMarca cn_agregarMarca = new CN_frmAgregarMarca();
        private string nombreMarca_primero;

        public frmAgregarMarca()
        {
            InitializeComponent();
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtNombre.Focus();

            if ((string)this.Tag == "Actualizar")
            {
                lblMarca.Content = "Actualizar Categoria";
                btnAgregarMarca.Content = "Actualizar Categoria";
                txtCodigo.Visibility = Visibility.Visible;
                lblCodigo.Visibility = Visibility.Visible;

                DataRowView filaSeleccionada = (DataRowView)ventanaProducto.dgMarca.SelectedItem;

                string Id = filaSeleccionada["idMarca"].ToString();
                string nombre = filaSeleccionada["nombre_marca"].ToString();

                nombreMarca_primero = nombre;

                txtCodigo.Text = Id;
                txtNombre.Text = nombre;

            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void btnAgregarMarca_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("No completó el campo obligatorio Nombre", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Focus();
                return;
            }
            string idMarca = txtCodigo.Text;
            string nombreMarca = txtNombre.Text;
            CN_frmAgregarMarca Marca = new CN_frmAgregarMarca(idMarca, nombreMarca);

            if ((string)this.Tag != "Actualizar")
            {
                if (!cn_agregarMarca.verificarExistencia(nombreMarca))
                {
                    MessageBox.Show("La Marca ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNombre.Focus();
                    return;
                }
                if (!Marca.SubirMarca())
                {
                    MessageBox.Show("Ocurrio un error al intentar insertar la Marca", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (ventanaProducto != null)
                    {
                        ventanaProducto.CargarMarca();
                        ventanaProducto.CargarlistaMarca();
                    }
                    MessageBox.Show("La marca se subió correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNombre.Text = "";
                    txtNombre.Focus();
                }
            }
            else
            {
                if (nombreMarca_primero != nombreMarca && !cn_agregarMarca.verificarExistencia(nombreMarca))
                {
                    MessageBox.Show("La marca ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNombre.Focus();
                    return;
                }
                if (!Marca.ActualizarMarca())
                {
                    MessageBox.Show("Error al intentar Actualizar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (ventanaProducto != null)
                    {
                        ventanaProducto.CargarMarca();
                        ventanaProducto.CargarlistaMarca();
                    }
                    MessageBox.Show("La marca se Actualizó correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nombreMarca_primero = nombreMarca;
                    txtNombre.Focus();
                }
            }
}
        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
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
        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter) btnAgregarMarca_Click(sender, e);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void nombre_Click(object sender, MouseButtonEventArgs e)
        {
            txtNombre.Focus();
        }
        private void txtNombre_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbNombre.Visibility = Visibility.Collapsed;
        }

        private void txtNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) txtbNombre.Visibility = Visibility.Visible;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Escape) Close();
            else if (e.Key == Key.Enter) btnAgregarMarca_Click(sender, e);
        }
    }
}
