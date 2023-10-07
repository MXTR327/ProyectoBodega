using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Datos
{
    public class CD_frmAgregarPaqueteProductos
    {
        private SQLiteCommand cmd;
        public bool SubirProductoDB(string nombreProducto, string descripcion, double precioCompra, double precioVenta, string medida, int stock, string idCategoria, string idProveedor, string idMarca)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();

                string sql = "INSERT INTO producto (nombre_producto, descripcion, precio_compra, precio_venta, medida, stock, Categoria_idCategoria, Proveedor_idProveedor, marca_idMarca) " +
                                 "VALUES (@nombre, @descripcion, @precio_compra, @precio_venta, @medida, @stock, @Categoria_idCategoria, @Proveedor_idProveedor, @marca_idMarca)";

                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@nombre", nombreProducto);
                cmd.Parameters.AddWithValue("@descripcion", string.IsNullOrEmpty(descripcion) ? "Sin descripcion" : descripcion);
                cmd.Parameters.AddWithValue("@precio_compra", precioCompra);
                cmd.Parameters.AddWithValue("@precio_venta", precioVenta);
                cmd.Parameters.AddWithValue("@medida", medida);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@Categoria_idCategoria", idCategoria);
                cmd.Parameters.AddWithValue("@Proveedor_idProveedor", idProveedor);
                cmd.Parameters.AddWithValue("@marca_idMarca", idMarca);
                cmd.ExecuteNonQuery();
                rpta = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return rpta;
        }
        public bool VerSiNoExisteProducto(string nombreProducto)
        {
            bool Noexiste = true;
            try
            {
                Conexion.Conectar();

                string sql = "SELECT COUNT(*) FROM producto WHERE nombre_producto = @textoEntrada";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@textoEntrada", nombreProducto.Trim());
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0) { Noexiste = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return Noexiste;
        }
    }
}
