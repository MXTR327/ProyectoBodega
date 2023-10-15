using Negocio;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoBodega
{
    public partial class frmAgregarVendedor : Window
    {
        internal ventanaEmpleados ventanaEmpleados;

        public frmAgregarVendedor()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsuario.Focus();
            if ((string)this.Tag == "Actualizar")
            {
                lblFrm.Content = "Actualizar Vendedor";
                btnAgregarVendedor.Content = "Actualizar Vendedor";
                txtCodigo.Visibility = Visibility.Visible;
                lblCodigo.Visibility = Visibility.Visible;

                txtbUsuario.Visibility = Visibility.Collapsed;
                txtbNombre.Visibility = Visibility.Collapsed;
                if (string.IsNullOrWhiteSpace(txtDireccion.Text))
                {
                    txtbDireccion.Visibility = Visibility.Collapsed;
                }
                if (string.IsNullOrWhiteSpace(txtTelefono.Text))
                {
                    txtbTelefono.Visibility = Visibility.Collapsed;
                }

                //----------------------------------------------------------------------------------------\\
                DataRowView filaSeleccionada = (DataRowView)ventanaEmpleados.dgVendedores.SelectedItem;

                string Id = filaSeleccionada["idVendedor"].ToString();
                string nombre = filaSeleccionada["nombre_vendedor"].ToString();
                string telefono = filaSeleccionada["telefono"].ToString();
                string direccion = filaSeleccionada["direccion"].ToString();
                string usuario = filaSeleccionada["usuario"].ToString();

                nombreVendedor_primero = nombre;

                txtCodigo.Text = Id;
                txtUsuario.Text = usuario;
                txtNombre.Text = nombre;

                if (!string.IsNullOrEmpty(telefono))
                {
                    chkTelefono.IsChecked = true;
                    txtTelefono.IsEnabled = true;
                    txtTelefono.Text= telefono;
                    gridTelefono.Cursor = Cursors.IBeam;
                }
                if (!string.IsNullOrEmpty(direccion))
                {
                    chkDireccion.IsChecked = true;
                    txtDireccion.IsEnabled = true;
                    txtDireccion.Text = direccion;
                    gridDireccion.Cursor = Cursors.IBeam;
                }
                txtUsuario.Focus();
            }
        }
        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void chkDireccion_Click(object sender, RoutedEventArgs e)
        {
            txtDireccion.IsEnabled = !txtDireccion.IsEnabled;
            txtDireccion.Text = "";
            if (txtDireccion.IsEnabled == IsEnabled)
            {
                gridDireccion.Cursor = Cursors.IBeam;
                txtDireccion.Focus();
            }
            else
            {
                gridDireccion.Cursor = Cursors.Arrow;
                txtbDireccion.Visibility = Visibility.Visible;
            }
        }
        private void chkTelefono_Click(object sender, RoutedEventArgs e)
        {
            txtTelefono.IsEnabled = !txtTelefono.IsEnabled;
            txtTelefono.Text = "";
            if (txtTelefono.IsEnabled == true)
            {
                gridTelefono.Cursor = Cursors.IBeam;
                txtTelefono.Focus();
            }
            else
            {
                gridTelefono.Cursor = Cursors.Arrow;
                txtbTelefono.Visibility = Visibility.Visible;
            }
        }
        private void primeraLetraMayuscula_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = textBox.Text.First().ToString().ToUpper() + textBox.Text.Substring(1);
                textBox.Text = textBox.Text;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
        private void txtTelefono_PreviewKeyDown(object sender, KeyEventArgs e)
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
        CN_frmAgregarVendedor cn_vendedor = new CN_frmAgregarVendedor();
        //------------------------------------------------------------------------------------------------------------------------------\\
        public string nombreVendedor_primero;
        private void btnAgregarVendedor_Click(object sender, RoutedEventArgs e)
        {
            string id= txtCodigo.Text;
            string usuario = txtUsuario.Text;
            string nombre = txtNombre.Text;
            string telefono = txtTelefono.Text;
            string direccion = txtDireccion.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("No puede dejar campos obligatorios vacios", "Error");
                txtUsuario.Focus();
                return;
            }

            CN_frmAgregarVendedor cn_agregarvendedor = new CN_frmAgregarVendedor(id, usuario, nombre, telefono, direccion);

            if((string)this.Tag == "Crear")
            {
                if (cn_agregarvendedor.verificarExistencia())
                {
                    if (cn_agregarvendedor.AgregarVendedor())
                    {
                        MessageBox.Show("El vendedor se agrego correctamente", "Exito!!");
                        txtUsuario.Text = string.Empty;
                        txtNombre.Text = string.Empty;
                        txtTelefono.Text = string.Empty;
                        txtDireccion.Text = string.Empty;
                        txtUsuario.Focus();

                        if (ventanaEmpleados != null)
                        {
                            ventanaEmpleados.CargarVendedores();

                            if (ventanaEmpleados.dgVendedores.Items.Count > 0)
                            {
                                ventanaEmpleados.dgVendedores.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ocurrio un error al intentar insertar el vendedor", "Error");
                        txtUsuario.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Ya existe un vendedor con este usuario", "Error");
                    txtUsuario.Focus();
                }
            }
            else
            {
                if (nombreVendedor_primero != nombre)
                {
                    cn_vendedor.Usuario = usuario;
                    if (!cn_vendedor.verificarExistencia())
                    {
                        MessageBox.Show("El proveedor ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        txtNombre.Focus();
                        return;
                    }
                }
                if (cn_agregarvendedor.ActualizarVendedor())
                {
                    if (ventanaEmpleados != null)
                    {
                        ventanaEmpleados.CargarVendedores();
                    }
                    MessageBox.Show("El vendedor se Actualizó correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    nombreVendedor_primero = nombre;
                    txtNombre.Focus();
                }
                else
                {
                    MessageBox.Show("Error al intentar Actualizar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtNombre.Focus();
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbUsuario.Visibility = Visibility.Collapsed;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtbUsuario.Visibility = Visibility.Visible;
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
        private void txtTelefono_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbTelefono.Visibility = Visibility.Collapsed;
        }
        private void txtTelefono_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                txtbTelefono.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtDireccion_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbDireccion.Visibility = Visibility.Collapsed;
        }
        private void txtDireccion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                txtbDireccion.Visibility = Visibility.Visible;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnAgregarVendedor_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                BtnSalir_Click(sender, e);
            }
        }
    }
}
