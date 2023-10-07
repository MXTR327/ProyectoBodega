using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Datos
{
    public class CD_ventanaEmpleados
    {
        public DataTable dt;
        public SQLiteDataAdapter da;
        public SQLiteCommand cmd;
        public bool BorrarFilaDB(string idFila)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();

                string sql = $"DELETE FROM vendedor WHERE idVendedor = @id_a_borrar";

                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@id_a_borrar", idFila);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    rpta = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return rpta;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        public bool BorrarVendedorDB(string idVendedor)
        {
            return BorrarFilaDB(idVendedor);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
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
            return ConseguirTabla($"SELECT * FROM vendedor WHERE nombre_vendedor LIKE '%{filtro}%'");
        }
    }
}
