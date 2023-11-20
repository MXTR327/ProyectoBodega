using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoBodega
{
    public partial class BuscarCombobox : Window
    {
        internal frmAgregarProducto frmAgregarProducto;

        internal frmAgregarPaqueteProductos frmAgregarPaqueteProductos;

        internal VentanaProductos VentanaProductos;

        CN_BuscarCombobox cn_buscar = new CN_BuscarCombobox();
        private string filtro;
        

        public BuscarCombobox()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (frmAgregarProducto != null || frmAgregarPaqueteProductos != null || VentanaProductos != null)
            {
                lblTitulo.Content = "Elegir "+(string)this.Tag;
                
                if ((string)this.Tag == "categoria")
                {
                    colD.Binding = new Binding("idCategoria");
                    colNombre.Binding = new Binding("nombre_categoria");
                    DataGridTextColumn nuevaColumna = new DataGridTextColumn();
                    nuevaColumna.Header = "Descripcion";
                    Binding binding = new Binding("descripcion");
                    nuevaColumna.Binding = binding;
                    dgTabla.Columns.Add(nuevaColumna);
                }
                else if ((string)this.Tag == "proveedor")
                {
                    colD.Binding = new Binding("idProveedor");
                    colNombre.Binding = new Binding("nombre_proveedor");

                    DataGridTextColumn nuevaColumna = new DataGridTextColumn();
                    nuevaColumna.Header = "Direccion";
                    Binding binding = new Binding("direccion_provedor");
                    nuevaColumna.Binding = binding;
                    dgTabla.Columns.Add(nuevaColumna);

                    DataGridTextColumn nuevaColumna2 = new DataGridTextColumn();
                    nuevaColumna2.Header = "Telefono";
                    Binding binding2 = new Binding("numero_contacto");
                    nuevaColumna2.Binding = binding2;
                    dgTabla.Columns.Add(nuevaColumna2);
                }
                else if ((string)this.Tag == "marca")
                {
                    colD.Binding = new Binding("idMarca");
                    colNombre.Binding = new Binding("nombre_marca");
                }
                filtro = "";
                Cargar();
            }
        }
        public void Cargar()
        {
            DataTable dt = cn_buscar.tblProducto(filtro,(string)this.Tag);
            dgTabla.ItemsSource = dt.DefaultView;
        }
        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
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

            filtro = txtBuscador.Text;
            Cargar();
        }
        private void AsignarValorComboBox(ComboBox cmb, string columna)
        {
            if (cmb != null && columna != null)
            {
                DataRowView filaSeleccionada = (DataRowView)dgTabla.SelectedItem;
                if (filaSeleccionada == null)
                {
                    MessageBox.Show("Seleccione una fila a seleccionar", "Error");
                    return;
                }
                string Id = filaSeleccionada[columna].ToString();
                cmb.SelectedValue = Id;
            }
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if ((string)this.Tag == "categoria")
            {
                AsignarValorComboBox(frmAgregarProducto?.cmbCategoria, "idCategoria");
                AsignarValorComboBox(frmAgregarPaqueteProductos?.cmbCategoria, "idCategoria");
                AsignarValorComboBox(VentanaProductos?.cmbCategoria, "idCategoria");
            }
            else if ((string)this.Tag == "proveedor")
            {
                AsignarValorComboBox(frmAgregarProducto?.cmbProveedor, "idProveedor");
                AsignarValorComboBox(frmAgregarPaqueteProductos?.cmbProveedor, "idProveedor");
                AsignarValorComboBox(VentanaProductos?.cmbProveedor, "idProveedor");
            }
            else if ((string)this.Tag == "marca")
            {
                AsignarValorComboBox(frmAgregarProducto?.cmbMarca, "idMarca");
                AsignarValorComboBox(frmAgregarPaqueteProductos?.cmbMarca, "idMarca");
                AsignarValorComboBox(VentanaProductos?.cmbMarca, "idMarca");
            }

            Close();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void dgTabla_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnAceptar_Click(sender, e);
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            else if (e.Key == Key.Enter) btnAceptar_Click(sender, e);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Ventana.Cursor = Cursors.SizeAll;
                DragMove();
                Ventana.Cursor = Cursors.Arrow;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        private void txtBuscadorMarca_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbBuscar.Visibility = Visibility.Collapsed;
        }
        private void txtBuscadorMarca_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscador.Text)) txtbBuscar.Visibility = Visibility.Visible;
        }

        
    }
}
