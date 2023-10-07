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
    public class CD_ventanaVentas
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
        public DataTable tablaVentas(string fechaCompleta)
        {
            return ConseguirTabla($@"SELECT idVenta, fecha, hora, nombre_cliente, nombre_vendedor, Total_Venta, ganancia, Vuelto
                                FROM venta 
                                LEFT JOIN vendedor ON venta.Vendedor_idVendedor = vendedor.idVendedor    
                                WHERE fecha LIKE '%{fechaCompleta}%'");
        }
        public DataTable tablaDetalleVentas(string idVenta)
        {
            return ConseguirTabla($@"SELECT dv.idDetalle_venta, 
                                    dv.producto_idProducto as idProducto, 
                                    p.nombre_producto as nombre_producto, 
                                    dv.Cantidad, dv.Precio_Unitario
                                    FROM Detalle_venta dv
                                    LEFT JOIN producto p ON dv.producto_idProducto = p.idProducto
                                    WHERE venta_idVenta = '{idVenta}'");
        }
        public DataTable detalleProducto(string idProducto)
        {
            return ConseguirTabla($@"SELECT p.nombre_producto, 
                                    p.descripcion, 
                                    p.precio_compra, 
                                    p.stock, 
                                    p.precio_venta, 
                                    p.medida, 
                                    c.nombre_categoria, 
                                    prov.nombre_proveedor, 
                                    m.nombre_marca
                            FROM producto p
                            LEFT JOIN categoria c ON p.Categoria_idCategoria = c.idCategoria
                            LEFT JOIN proveedor prov ON p.Proveedor_idProveedor = prov.idProveedor
                            LEFT JOIN marca m ON p.marca_idMarca = m.idMarca
                            WHERE p.idProducto = {idProducto}");
        }
    }
}
