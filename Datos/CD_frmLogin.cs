using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Datos
{
    public class CD_frmLogin
    {
        SQLiteCommand cmd;
        SQLiteDataAdapter adapter;
        public DataTable VerificarExistencia(string usuario)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string consultaSql = "SELECT idVendedor,nombre_vendedor FROM vendedor WHERE usuario = @Usuario";
                Conexion.Conectar();
                cmd = new SQLiteCommand(consultaSql, Conexion.con);
                cmd.Parameters.AddWithValue("@Usuario", usuario);

                adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(resultTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error INSERTAR");
            }
            return resultTable;
        }
    }
}
