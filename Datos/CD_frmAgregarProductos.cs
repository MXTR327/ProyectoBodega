using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Datos
{
    public class CD_frmAgregarProductos
    {
        private SQLiteCommand cmd;
        public bool VerSiNoExisteProducto(string texto_entrada)
        {
            bool Noexiste = true;
            try
            {
                Conexion.Conectar();

                string sql = "SELECT COUNT(*) FROM producto WHERE nombre_producto = @textoEntrada";
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@textoEntrada", texto_entrada.Trim());
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0) { Noexiste = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return Noexiste;
        }
        public bool SubirProductoDB(string nombre_producto, string descripcion, decimal precio_compra, decimal precio_venta, string medida, int stock, string nombre_Categoria, string nombre_Proveedor, string nombre_Marca)
        {
            bool rpta = false;
            try
            {
                Conexion.Conectar();

                string sql = "INSERT INTO producto (nombre_producto, descripcion, precio_compra, precio_venta, medida, stock, Categoria_idCategoria, Proveedor_idProveedor, marca_idMarca) " +
                                 "VALUES (@nombre, @descripcion, @precio_compra, @precio_venta, @medida, @stock, @Categoria_idCategoria, @Proveedor_idProveedor, @marca_idMarca)";

                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@nombre", nombre_producto);
                cmd.Parameters.AddWithValue("@descripcion", string.IsNullOrEmpty(descripcion) ? "Sin descripcion" : descripcion);
                cmd.Parameters.AddWithValue("@precio_compra", precio_compra);
                cmd.Parameters.AddWithValue("@precio_venta", precio_venta);
                cmd.Parameters.AddWithValue("@medida", medida);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@Categoria_idCategoria", nombre_Categoria);
                cmd.Parameters.AddWithValue("@Proveedor_idProveedor", nombre_Proveedor);
                cmd.Parameters.AddWithValue("@marca_idMarca", nombre_Marca);
                cmd.ExecuteNonQuery();
                rpta = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return rpta;
        }
    }
}
