using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Datos
{
    public class CD_VentanaAgregarStock
    {
        public DataTable dt;
        public SQLiteDataAdapter da;
        public SQLiteCommand cmd;

        public DataTable ConseguirTabla(string consulta)
        {
            try
            {
                string consultasql = consulta;
                Conexion.Conectar();
                da = new SQLiteDataAdapter(consultasql, Conexion.con);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return dt;
        }
        public DataTable tablaProductos(string filtro)
        {
            return ConseguirTabla($"SELECT idProducto,nombre_producto,stock,precio_venta FROM producto WHERE nombre_producto like '%{filtro}%' ORDER BY stock ASC");
        }
    }
}
