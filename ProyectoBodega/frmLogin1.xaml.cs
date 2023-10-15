using Negocio;
using ProyectoBodega;
using System.Data;
using System.Windows;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace Presentacion
{
    public partial class frmLogin1 : Window
    {
        CN_frmLogin cn_frmlogin = new CN_frmLogin();
        public frmLogin1()
        {
            InitializeComponent();
        }
        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarDatos())
            {
                DataTable dt = new DataTable();

                cn_frmlogin.Usuario = txtUsuario.Text;

                dt = cn_frmlogin.VerificarUsuario();

                if (dt.Rows.Count > 0)
                {
                    string id = dt.Rows[0][0].ToString();
                    string nombre = dt.Rows[0][1].ToString();

                    index ventanaprincipal = new index();
                    ventanaprincipal.idvendedor = id;
                    ventanaprincipal.nombrevendedor = nombre;

                    ((App)System.Windows.Application.Current).ChangeMainWindow(ventanaprincipal);
                }
                else
                {
                    MessageBox.Show("Acceso NO AUTORIZADO", "Mensaje");
                }
            }
        }
        private bool ValidarDatos()
        {
            bool rpta = true;
            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                MessageBox.Show("Debe rellenar el campo usuario", "Error");
                txtUsuario.Focus();
                rpta = false;
            }
            return rpta;
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            frmAgregarVendedor agregarVendedor = new frmAgregarVendedor();
            agregarVendedor.Tag = "Crear";
            agregarVendedor.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsuario.Focus();
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnIngresar_Click(sender, e);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            placeholderText.Visibility = Visibility.Collapsed;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                placeholderText.Visibility = Visibility.Visible;
            }
        }
        private void placeholderText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtUsuario.Focus();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
            else if (e.Key == Key.Enter)
            {
                BtnIngresar_Click(sender, e);
            }
            else if(e.Key == Key.F1)
            {
                btnRegistrar_Click(sender, e);
            }
        }
    }
}
