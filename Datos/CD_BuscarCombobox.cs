using System;
using System.Windows;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Datos
{
    public class CD_BuscarCombobox
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
        public DataTable tabla(object filtro, string tag)
        {
            DataTable tabla = ConseguirTabla($"SELECT * FROM {tag}");
            string fila_1 = ""; 
            if (tabla.Columns.Count > 1)
            {
                fila_1 = tabla.Columns[1].ColumnName;
                return ConseguirTabla($"SELECT * FROM {tag} WHERE {fila_1} LIKE '%{filtro}%'");
            }
            else
            {
                MessageBox.Show("No hay suficientes columnas en la tabla", "Error");
                return null;
            }
        }
    }
}
