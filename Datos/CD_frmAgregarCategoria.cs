using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Datos
{
    public class CD_frmAgregarCategoria
    {
        private SQLiteCommand cmd;

        public bool ActualizarCategoriaDB(string idCategoria, string nombreCategoria, string descripcionCategoria)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();
                string sql = "UPDATE categoria SET nombre_categoria = @nombre, descripcion = @descripcion WHERE idCategoria = @idCategoria";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@idCategoria", idCategoria);
                cmd.Parameters.AddWithValue("@nombre", nombreCategoria);
                cmd.Parameters.AddWithValue("@descripcion", descripcionCategoria);
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

        public bool AgregarCategoriaDB(string nombre_categoria, string descripcion_categoria)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();
                string sql = "INSERT INTO categoria (nombre_categoria, descripcion) VALUES (@nombre, @descripcion)";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@nombre", nombre_categoria);
                cmd.Parameters.AddWithValue("@descripcion", descripcion_categoria);
                int rowsAffected = cmd.ExecuteNonQuery();
                rpta = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return rpta;
        }
        public bool VerSiNoExisteCategoria(string nombre_categoria)
        {
            bool Noexiste = true;
            try
            {
                Conexion.Conectar();
                string sql = "SELECT COUNT(*) FROM categoria WHERE nombre_categoria = @textoEntrada";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@textoEntrada", nombre_categoria.Trim());
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
