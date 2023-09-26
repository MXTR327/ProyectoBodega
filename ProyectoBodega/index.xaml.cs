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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Negocio;

namespace ProyectoBodega
{
    public partial class index : Window
    {
        CN_index cn_index = new CN_index();

        internal string idvendedor;
        internal string nombrevendedor;
        private string filtroProductos;

        public index()
        {
            InitializeComponent();
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ventanaIndex_Loaded(object sender, RoutedEventArgs e)
        {
            txtIdVendedor.Text = idvendedor;
            txtNombreVendedor.Text = nombrevendedor;

            filtroProductos = "";
            CargarProducto();

            //DataGridViewCellEventArgs cellClickArgs = new DataGridViewCellEventArgs(1, 0);
            //if (dgvProducto.RowCount > 0)
            //{
            //    dgvProducto_CellClick(dgvProducto, cellClickArgs);
            //}
        }
        public void CargarProducto()
        {
            DataTable dt = cn_index.tblProducto(filtroProductos);
            dgProducto.ItemsSource = dt.DefaultView;
        }

        private void txtBuscadorProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            filtroProductos = txtBuscadorProducto.Text;
            CargarProducto();
        }
    }
}
