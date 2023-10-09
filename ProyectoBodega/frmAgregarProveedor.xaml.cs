using Negocio;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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


                txtbNombre.Visibility = Visibility.Collapsed;

                if (string.IsNullOrWhiteSpace(txtDireccion.Text))
                {
                    txtbDireccion.Visibility = Visibility.Collapsed;
                }
                if (string.IsNullOrWhiteSpace(txtNumero.Text))
                {
                    txtbNumero.Visibility = Visibility.Collapsed;
                }
                //------------------------------------------------------------------------------------------------------------------------------\\
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
                    chkNumero.IsChecked = true;
                    txtNumero.IsReadOnly = false;
                    txtNumero.Text = numero;
                }
                if (!string.IsNullOrEmpty(direccion))
                {
                    chkDireccion.IsChecked = true;
                    txtDireccion.IsReadOnly = false;

                    txtDireccion.Text = direccion;
                }
                txtNombre.Focus();
            }
        }
        private void chkDireccion_Click(object sender, RoutedEventArgs e)
        {
            txtDireccion.Text = "";
            txtDireccion.IsReadOnly = !txtDireccion.IsReadOnly;
            if (txtDireccion.IsReadOnly == false)
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
        private void chkNumero_Click(object sender, RoutedEventArgs e)
        {
            txtNumero.Text = "";
            txtNumero.IsReadOnly = !txtNumero.IsReadOnly;
            if (txtNumero.IsReadOnly == false)
            {
                gridNumero.Cursor = Cursors.IBeam;
                txtNumero.Focus();
            }
            else
            {
                gridNumero.Cursor = Cursors.Arrow;
                txtbNumero.Visibility = Visibility.Visible;
            }
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
                MessageBox.Show("No completó el campo obligatorio Nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        MessageBox.Show("El proveedor se subió correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtNombre.Text = "";
                        txtDireccion.Text = "";
                        txtNumero.Text = "";
                        txtNombre.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Error al intentar Insertar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        txtNombre.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("El proveedor ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtNombre.Focus();
                }
            }
            else
            {
                if (nombreProveedor_primero != nombreProveedor)
                {
                    if (!cn_frmproveedor.VerificarExistencia(nombreProveedor))
                    {
                        MessageBox.Show("El proveedor ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show("La Categoria se Actualizó correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    nombreProveedor_primero = nombreProveedor;
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
        private void primeraLetraMayuscula_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                string newText = char.ToUpper(textBox.Text[0]) + textBox.Text.Substring(1).ToLower();
                textBox.Text = newText;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
        private void txtTelefono_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) && e.Key != Key.Back || (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                e.Handled = true;
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
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void nombre_Click(object sender, MouseButtonEventArgs e)
        {
            txtNombre.Focus();
        }
        private void txtNombre_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbNombre.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                txtbDireccion.Visibility = Visibility.Visible;
            }
            if (string.IsNullOrWhiteSpace(txtNumero.Text))
            {
                txtbNumero.Visibility = Visibility.Visible;
            }
        }
        private void txtNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                txtbNombre.Visibility = Visibility.Visible;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void direccion_Click(object sender, MouseButtonEventArgs e)
        {
            if (chkDireccion.IsChecked == true)
            {
                txtDireccion.Focus();
            }
        }
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
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void numero_Click(object sender, MouseButtonEventArgs e)
        {
            if (chkNumero.IsChecked == true)
            {
                txtNumero.Focus();
            }
        }
        private void txtNumero_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbNumero.Visibility = Visibility.Collapsed;
        }
        private void txtNumero_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumero.Text))
            {
                txtbNumero.Visibility = Visibility.Visible;
            }
        }
    }
}
