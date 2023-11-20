using System;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;

namespace Datos
{
    public class CD_frmCobrar
    {
        private SQLiteCommand cmd;

        public bool actualizarVenta(int idVenta, decimal gananciaTotal)
        {
            try
            {
                Conexion.Conectar();

                string sql = @"UPDATE venta SET ganancia = @ganancia WHERE idVenta = @idVenta";

                SQLiteCommand cmd = new SQLiteCommand(sql, Conexion.con);

                cmd.Parameters.AddWithValue("@idVenta", idVenta);
                cmd.Parameters.AddWithValue("@ganancia", Convert.ToDouble(gananciaTotal));

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return false;
            }
        }

        public int NuevaVenta(string fecha, string hora, string nombreCliente, int idVendedor, decimal totalVenta, decimal ganancia, decimal vuelto)
        {
            int id = -1;
            try
            {
                Conexion.Conectar();

                string sql = @"INSERT INTO venta (fecha, hora, nombre_cliente, Vendedor_idVendedor, Total_venta, ganancia, vuelto) 
                                VALUES (@fecha, @hora, @nombreCliente, @idVendedor, @totalVenta, @gananciaVenta, @vueltoVenta);
                                SELECT last_insert_rowid();";

                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@hora", hora);
                cmd.Parameters.AddWithValue("@nombreCliente", nombreCliente);
                cmd.Parameters.AddWithValue("@idVendedor", idVendedor);
                cmd.Parameters.AddWithValue("@totalVenta", totalVenta);
                cmd.Parameters.AddWithValue("@gananciaVenta", Convert.ToDouble(ganancia));
                cmd.Parameters.AddWithValue("@vueltoVenta", Convert.ToDouble(vuelto));

                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return id;
        }

        public decimal NuevoDetalleVenta(int idVenta, int idProducto, int cantidadProducto, decimal precioUnitarioProducto)
        {
            decimal gananciaProducto = 0.0m;
            try
            {
                Conexion.Conectar();

                string obtenerCostoProductoSQL = "SELECT precio_compra FROM producto WHERE idProducto = @idProducto";
                cmd = new SQLiteCommand(obtenerCostoProductoSQL, Conexion.con);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                decimal costoProducto = Convert.ToDecimal(cmd.ExecuteScalar());

                string sql = @"INSERT INTO Detalle_venta (venta_idVenta, producto_idProducto, cantidad, Precio_unitario) 
                VALUES (@venta_idVenta, @producto_idProducto, @cantidad, @Precio_unitario)";

                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@venta_idVenta", idVenta);
                cmd.Parameters.AddWithValue("@producto_idProducto", idProducto);
                cmd.Parameters.AddWithValue("@cantidad", cantidadProducto);
                cmd.Parameters.AddWithValue("@Precio_unitario", Convert.ToDouble(precioUnitarioProducto));
                cmd.ExecuteNonQuery();

                gananciaProducto = (precioUnitarioProducto - costoProducto) * cantidadProducto;

                string actualizarStock = @"UPDATE producto SET stock = stock - @cantidadProducto WHERE idProducto = @idProducto";
                cmd.CommandText = actualizarStock;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@cantidadProducto", cantidadProducto);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return gananciaProducto;
        }
    }
}
