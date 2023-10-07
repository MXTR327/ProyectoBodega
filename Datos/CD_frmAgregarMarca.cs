using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Datos
{
    public class CD_frmAgregarMarca
    {
        private SQLiteCommand cmd;

        public bool ActualizarMarcaDB(string idMarca, string nombreMarca)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();
                string sql = "UPDATE marca SET nombre_marca = @nombre WHERE idMarca = @idMarca";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@idMarca", idMarca);
                cmd.Parameters.AddWithValue("@nombre", nombreMarca);
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

        public bool AgregarMarcaDB(string nombre)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();
                string sql = "INSERT INTO marca (nombre_marca) VALUES (@nombre)";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                int rowsAffected = cmd.ExecuteNonQuery();
                rpta = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return rpta;
        }
        public bool VerSiNoExisteMarca(string nombre_marca)
        {
            bool Noexiste = true;
            try
            {
                Conexion.Conectar();
                string sql = "SELECT COUNT(*) FROM marca WHERE nombre_marca = @textoEntrada";

                cmd = new SQLiteCommand(sql, Conexion.con);

                cmd.Parameters.AddWithValue("@textoEntrada", nombre_marca.Trim());
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
