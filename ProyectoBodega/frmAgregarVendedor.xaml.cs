using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Negocio;

namespace ProyectoBodega
{
    public partial class frmAgregarVendedor : Window
    {
        private object ventanaEmpleados;

        public frmAgregarVendedor()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void chkDireccion_Click(object sender, RoutedEventArgs e)
        {
            txtDireccion.Text = "";
            txtDireccion.IsEnabled = !txtDireccion.IsEnabled;
        }
        private void chkTelefono_Click(object sender, RoutedEventArgs e)
        {
            txtTelefono.Text = "";
            txtTelefono.IsEnabled = !txtTelefono.IsEnabled;
        }
        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = (System.Windows.Controls.TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = textBox.Text.First().ToString().ToUpper() + textBox.Text.Substring(1);
                textBox.Text = textBox.Text;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
        private void txtTelefono_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) && e.Key != Key.Back || (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                e.Handled = true;
            }
        }
        private void btnAgregarVendedor_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text;
            string nombre = txtNombre.Text;
            string telefono = txtTelefono.Text;
            string direccion = txtDireccion.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(nombre))
            {
                System.Windows.MessageBox.Show("No puede dejar campos obligatorios vacios", "Error");
                txtUsuario.Focus();
                return;
            }
            CN_frmAgregarVendedor cn_agregarvendedor = new CN_frmAgregarVendedor(usuario, nombre, telefono, direccion);
            if (cn_agregarvendedor.verificarExistencia())
            {
                if (cn_agregarvendedor.AgregarVendedor())
                {
                    System.Windows.MessageBox.Show("El vendedor se agrego correctamente", "Exito!!");
                    txtUsuario.Text = string.Empty;
                    txtNombre.Text = string.Empty;
                    txtTelefono.Text = string.Empty;
                    txtDireccion.Text = string.Empty;
                    txtUsuario.Focus();

                    //if (ventanaEmpleados != null)
                    //{
                    //    ventanaEmpleados.CargarVendedor();

                    //    DataGridViewCellEventArgs cellClickArgs = new DataGridViewCellEventArgs(1, 0);
                    //    if (ventanaEmpleados.dgvVendedores.RowCount > 0)
                    //    {
                    //        ventanaEmpleados.dgvVendedores_CellClick(ventanaEmpleados.dgvVendedores, cellClickArgs);
                    //    }
                    //}
                }
                else
                {
                    System.Windows.MessageBox.Show("Ocurrio un error al intentar insertar el vendedor", "Error");
                    txtUsuario.Focus();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Ya existe un vendedor con este usuario", "Error");
                txtUsuario.Focus();
            }
        }
    }
}
