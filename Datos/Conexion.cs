using System.Data;
using System.Data.SQLite;

namespace Datos
{
    public class Conexion
    {
        public static SQLiteConnection con = new SQLiteConnection("Data Source=BodegaDB.db;Version=3;");
        public static void Conectar()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }
        public static void Desconectar()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}
