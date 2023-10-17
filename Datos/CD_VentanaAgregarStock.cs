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

        public bool AgregarCantidadProductoDB(int idProducto, string cantidadAgregar)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();
                string sql = "UPDATE producto SET stock = stock + @cantidad WHERE idProducto = @idProducto";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                cmd.Parameters.AddWithValue("@cantidad", cantidadAgregar);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    rpta = true;
                }
                else
                {
                    rpta = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return rpta;
        }

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
