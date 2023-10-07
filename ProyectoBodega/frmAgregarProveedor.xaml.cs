using Negocio;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace ProyectoBodega
{
    public partial class frmAgregarProveedor : Window
    {
        internal VentanaProductos ventanaProducto;
        CN_frmAgregarProveedor cn_frmproveedor = new CN_frmAgregarProveedor();
        public frmAgregarProveedor()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtNombre.Focus();
            if ((string)this.Tag == "Actualizar")
            {
                lblProveedor.Content = "Actualizar Proveedor";
                btnAgregarProveedor.Content = "Actualizar Proveedor";
                txtCodigo.Visibility = Visibility.Visible;
                lblCodigo.Visibility = Visibility.Visible;

                DataRowView filaSeleccionada = (DataRowView)ventanaProducto.dgProveedor.SelectedItem;

                string Id = filaSeleccionada["idProveedor"].ToString();
                string nombre = filaSeleccionada["nombre_proveedor"].ToString();
                string direccion = filaSeleccionada["direccion_proveedor"].ToString();
                string numero = filaSeleccionada["numero_contacto"].ToString();

                nombreProveedor_primero = nombre;

                txtCodigo.Text = Id;
                txtNombre.Text = nombre;

                if (!string.IsNullOrEmpty(numero))
                {
                    chkContacto.IsChecked = true;
                    txtNumero.IsReadOnly = false;
                    txtNumero.Text = numero;
                }
                if (!string.IsNullOrEmpty(direccion))
                {
                    chkDireccion.IsChecked = true;
                    txtDireccion.IsReadOnly = false;

                    txtDireccion.Text = direccion;
                }
            }
        }
        private void chkDireccion_Click(object sender, RoutedEventArgs e)
        {
            txtDireccion.Text = "";
            txtDireccion.IsReadOnly = !txtDireccion.IsReadOnly;
        }
        private void chkContacto_Click(object sender, RoutedEventArgs e)
        {
            txtNumero.Text = "";
            txtNumero.IsReadOnly = !txtNumero.IsReadOnly;
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        public string nombreProveedor_primero;
        private void btnAgregarProveedor_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("No completó el campo obligatorio Nombre", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Focus();
                return;
            }
            string idProveedor = txtCodigo.Text;
            string nombreProveedor = txtNombre.Text;
            string direccionProveedor = txtDireccion.Text;
            string numeroContacto = txtNumero.Text;

            CN_frmAgregarProveedor proveedor = new CN_frmAgregarProveedor(idProveedor, nombreProveedor, direccionProveedor, numeroContacto);

            if ((string)this.Tag != "Actualizar")
            {
                if (cn_frmproveedor.VerificarExistencia(nombreProveedor))
                {
                    if (proveedor.SubirProveedor())
                    {
                        if (ventanaProducto != null)
                        {
                            ventanaProducto.CargarProveedor();
                            ventanaProducto.CargarlistaProveedor();
                        }
                        MessageBox.Show("El proveedor se subió correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtNombre.Text = "";
                        txtDireccion.Text = "";
                        txtNumero.Text = "";
                        txtNombre.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Error al intentar Insertar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtNombre.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("El proveedor ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNombre.Focus();
                }
            }
            else
            {
                if (nombreProveedor_primero != nombreProveedor)
                {
                    if (!cn_frmproveedor.VerificarExistencia(nombreProveedor))
                    {
                        MessageBox.Show("El proveedor ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtNombre.Focus();
                        return;
                    }
                }
                if (proveedor.ActualizarProveedor())
                {
                    if (ventanaProducto != null)
                    {
                        ventanaProducto.CargarProveedor();
                        ventanaProducto.CargarlistaProveedor();
                    }
                    MessageBox.Show("La Categoria se Actualizó correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nombreProveedor_primero = nombreProveedor;
                    txtNombre.Focus();
                }
                else
                {
                    MessageBox.Show("Error al intentar Actualizar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNombre.Focus();
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                string newText = char.ToUpper(textBox.Text[0]) + textBox.Text.Substring(1).ToLower();
                textBox.Text = newText;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
        private void txtDireccion_TextChanged(object sender, TextChangedEventArgs e)
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
        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnAgregarProveedor_Click(sender, e);
            }
        }


    }
}
