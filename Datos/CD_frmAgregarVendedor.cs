using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Datos
{
    public class CD_frmAgregarVendedor
    {
        private SQLiteCommand cmd;
        public bool insertarVendedor(string usuario, string nombre, string telefono, string direccion)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();
                string sql = "INSERT INTO vendedor (usuario,nombre_vendedor,telefono,direccion) VALUES (@usuario,@nombre,@telefono,@direccion)";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@telefono", telefono);
                cmd.Parameters.AddWithValue("@direccion", direccion);

                int rowsAffected = cmd.ExecuteNonQuery();
                rpta = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error INSERTAR");
            }
            return rpta;
        }

        public bool VerSiNoExisteVendedor(string nombre)
        {
            bool Noexiste = true;
            try
            {
                Conexion.Conectar();
                string sql = "SELECT COUNT(*) FROM vendedor WHERE usuario = @textoEntrada";

                cmd = new SQLiteCommand(sql, Conexion.con);

                cmd.Parameters.AddWithValue("@textoEntrada", nombre.Trim());


                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0) { Noexiste = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error EXISTECIA");
            }
            return Noexiste;
        }
    }
}
