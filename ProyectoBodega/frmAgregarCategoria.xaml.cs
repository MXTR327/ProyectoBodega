using Negocio;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoBodega
{
    public partial class frmAgregarCategoria : Window
    {
        CN_frmAgregarCategoria cn_frmagregarcategoria = new CN_frmAgregarCategoria();
        internal VentanaProductos ventanaProducto;

        public frmAgregarCategoria()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtNombre.Focus();

            if ((string)this.Tag != "Actualizar") return;

            lblCategoria.Content = "Actualizar Categoria";
            btnAgregar.Content = "Actualizar Categoria";
            txtCodigo.Visibility = Visibility.Visible;
            lblCodigo.Visibility = Visibility.Visible;

            DataRowView filaSeleccionada = (DataRowView)ventanaProducto.dgCategoria.SelectedItem;

            string Id = filaSeleccionada["idCategoria"].ToString();
            string nombre = filaSeleccionada["nombre_categoria"].ToString();
            string descripcion = filaSeleccionada["descripcion"].ToString();

            nombreCategoria_primero = nombre;

            txtCodigo.Text = Id;
            txtNombre.Text = nombre;

            if (string.IsNullOrEmpty(descripcion)) return;

            chkDescripcion.IsChecked = true;
            txtDescripcion.IsReadOnly = false;
            txtDescripcion.Text = descripcion;
        }
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
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //------------------------------------------------------------------------------------------------------------------------------\\
        public string nombreCategoria_primero;
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("No completó el campo obligatorio Nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombre.Focus();
                return;
            }
            string idCategoria = txtCodigo.Text;
            string nombreCategoria = txtNombre.Text;
            string descripcionCategoria = txtDescripcion.Text;

            CN_frmAgregarCategoria categoria = new CN_frmAgregarCategoria(idCategoria, nombreCategoria, descripcionCategoria);

            if ((string)this.Tag != "Actualizar")
            {
                if (!cn_frmagregarcategoria.VerificarExistencia(nombreCategoria))
                {
                    MessageBox.Show("La Categoria ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtNombre.Focus();
                    return;
                }
                if (!categoria.SubirCategoria())
                {
                    MessageBox.Show("Error al insertar Categorias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtNombre.Focus();
                }
                else
                {
                    if (ventanaProducto != null)
                    {
                        ventanaProducto.CargarProducto();
                        ventanaProducto.CargarCategoria();
                        ventanaProducto.CargarlistaCategoria();
                    }
                    MessageBox.Show("La Categoria se subió correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtNombre.Text = "";
                    txtDescripcion.Text = "";
                    txtNombre.Focus();
                }
            }
            else
            {
                if (nombreCategoria_primero != nombreCategoria && !cn_frmagregarcategoria.VerificarExistencia(nombreCategoria))
                {
                    MessageBox.Show("La Categoria ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtNombre.Focus();
                    return;
                }
                if (!categoria.ActualizarCategoria())
                {
                    MessageBox.Show("Error al intentar Actualizar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (ventanaProducto != null)
                {
                    ventanaProducto.CargarProducto();
                    ventanaProducto.CargarCategoria();
                    ventanaProducto.CargarlistaCategoria();
                }
                MessageBox.Show("La Categoria se Actualizó correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                nombreCategoria_primero = nombreCategoria;
                txtNombre.Focus();
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
        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter) btnAgregar_Click(sender, e);
        }
        private void txtDescripcion_TextChanged(object sender, TextChangedEventArgs e)
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
        private void txtNombre_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbNombre.Visibility = Visibility.Collapsed;
        }

        private void txtNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) txtbNombre.Visibility = Visibility.Visible;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtDescripcion_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbDescripcion.Visibility = Visibility.Collapsed;
        }
        private void txtDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text)) txtbDescripcion.Visibility = Visibility.Visible;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Escape) Close();
            else if(e.Key==Key.Enter) btnAgregar_Click(sender,e);
        }
    }
}
