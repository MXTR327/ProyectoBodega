using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Datos
{
    public class CD_index
    {
        public DataTable dt;
        public SQLiteDataAdapter da;
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
        public DataTable tablaProductosVendidos(string filtro)
        {
            return ConseguirTabla($@"SELECT 
                                RANK() OVER (ORDER BY COUNT(dv.venta_idVenta) DESC) AS Top,
                                p.idProducto, 
                                p.nombre_producto, 
                                p.precio_compra, 
                                p.precio_venta, 
                                p.stock, 
                                nombre_categoria, COUNT(dv.venta_idVenta) AS VecesVendido
                                FROM producto p
                                LEFT JOIN categoria c ON p.Categoria_idCategoria = c.idCategoria
                                LEFT JOIN Detalle_venta dv ON p.idProducto = dv.producto_idProducto
                                WHERE nombre_producto LIKE '%{filtro}%'
                                GROUP BY p.idProducto, p.nombre_producto, nombre_categoria
                                ORDER BY VecesVendido DESC");
        }
    }
}
