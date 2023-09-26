using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Datos
{
    public class Conexion
    {
        public static SQLiteConnection con = new SQLiteConnection("Data Source=max.db;Version=3;");
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
