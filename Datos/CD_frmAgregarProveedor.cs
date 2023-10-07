using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Datos
{
    public class CD_frmAgregarProveedor
    {
        private SQLiteCommand cmd;

        public bool ActualizarProveedorDB(string idProveedor, string nombreProveedor, string direccionProveedor, string numeroContacto)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();
                string sql = "UPDATE proveedor SET nombre_proveedor = @nombre, direccion_proveedor = @direccion, numero_contacto = @numero WHERE idProveedor = @idProveedor";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@idProveedor", idProveedor);
                cmd.Parameters.AddWithValue("@nombre", nombreProveedor);
                cmd.Parameters.AddWithValue("@direccion", direccionProveedor);
                cmd.Parameters.AddWithValue("@numero", numeroContacto);
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

        public bool AgregarProveedorDB(string nombreProveedor, string direccionProveedor, string numeroContacto)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();
                string sql = "INSERT INTO proveedor (nombre_proveedor, direccion_proveedor, numero_contacto) VALUES (@nombre, @direccion, @numero)";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@nombre", nombreProveedor);
                cmd.Parameters.AddWithValue("@direccion", direccionProveedor);
                cmd.Parameters.AddWithValue("@numero", numeroContacto);
                int rowsAffected = cmd.ExecuteNonQuery();
                rpta = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return rpta;
        }
        public bool VerSiNoExisteProveedor(string nombreProveedor)
        {
            bool Noexiste = true;
            try
            {
                Conexion.Conectar();
                string sql = "SELECT COUNT(*) FROM proveedor WHERE nombre_proveedor = @textoEntrada";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@textoEntrada", nombreProveedor.Trim());
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0) { Noexiste = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return Noexiste;
        }
    }
}
