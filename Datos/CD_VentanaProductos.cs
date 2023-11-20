using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Datos
{
    public class CD_VentanaProductos
    {
        public DataTable dt;
        public SQLiteDataAdapter da;
        public SQLiteCommand cmd;
        //------------------------------------------------------------------------------------------------------------------------------\\
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
        public DataTable tablaProductos(string filtro)
        {
            return ConseguirTabla($"SELECT idProducto,nombre_producto,precio_compra,precio_venta,stock, nombre_categoria FROM producto p LEFT JOIN categoria c ON p.Categoria_idCategoria = c.idCategoria WHERE nombre_producto like '%{filtro}%' ORDER BY stock ASC");
        }
        public DataTable tablaProveedor(string filtro)
        {
            return ConseguirTabla($"SELECT idProveedor,nombre_proveedor,direccion_proveedor,numero_contacto FROM proveedor WHERE nombre_proveedor like '%{filtro}%'");
        }
        public DataTable tablaCategoria(string filtro)
        {
            return ConseguirTabla($"SELECT idCategoria,nombre_categoria,descripcion FROM categoria WHERE nombre_categoria like '%{filtro}%'");
        }
        public DataTable tablaMarca(string filtro)
        {
            return ConseguirTabla($"SELECT idMarca,nombre_marca FROM marca WHERE nombre_marca like '%{filtro}%'");
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        public DataTable ObtenerDatos(string numero_id)
        {
            DataTable dataTable = new DataTable();
            string sql = $@"SELECT  idProducto, 
                                    nombre_producto, 
                                    descripcion, 
                                    precio_compra, 
                                    precio_venta, 
                                    medida, 
                                    stock, 
                                    Categoria_idCategoria, 
                                    Proveedor_idProveedor,
                                    Marca_idMarca 
                                    FROM producto WHERE idProducto = '{numero_id}';";

            try
            {
                Conexion.Conectar();
                da = new SQLiteDataAdapter(sql, Conexion.con);
                da.Fill(dataTable);
                Conexion.Desconectar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return dataTable;
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        public bool BorrarFilaDB(string tabla, string NombrefilaID, string idFila)
        {
            try
            {
                Conexion.Conectar();

                string sql = $"DELETE FROM {tabla} WHERE {NombrefilaID} = @id_a_borrar";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, Conexion.con))
                {
                    cmd.Parameters.AddWithValue("@id_a_borrar", idFila);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return false;
            }
        }
        public bool BorrarProductoDB(string idFila)
        {
            return BorrarFilaDB("producto", "idProducto", idFila);
        }
        public bool BorrarProveedorDB(string idFila)
        {
            return BorrarFilaDB("proveedor", "idProveedor", idFila);
        }
        public bool BorrarCategoriaDB(string idFila)
        {
            return BorrarFilaDB("categoria", "idCategoria", idFila);
        }
        public bool BorrarMarcaDB(string idFila)
        {
            return BorrarFilaDB("marca", "idMarca", idFila);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        public bool VerificarExistencia(string tabla, string campo, string valor)
        {
            bool noExiste = true;
            try
            {
                Conexion.Conectar();
                string sql = $"SELECT COUNT(*) FROM {tabla} WHERE {campo} = @valor";
                SQLiteCommand cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@valor", valor.Trim());
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    noExiste = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return noExiste;
        }

        public bool VerSiNoExisteProducto(string texto_entrada)
        {
            return VerificarExistencia("producto", "nombre_producto", texto_entrada);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        public bool ActualizarProductoDB(string idProducto, string nombreProducto, string descripcionProducto, decimal precioCompra, decimal precioVenta, string medida, string stock, string idCategoriaProducto, string idProveedorProducto, string idMarcaProducto)
        {
            try
            {
                string sql = @"UPDATE producto SET nombre_producto = @nombre, descripcion = @descripcion, precio_compra = @precio_compra, precio_venta = @precio_venta, medida = @medida, stock = @stock, Categoria_idCategoria = @Categoria_idCategoria, Proveedor_idProveedor = @Proveedor_idProveedor, marca_idMarca = @marca_idMarca WHERE idProducto = @idProducto";

                Conexion.Conectar();
                cmd = new SQLiteCommand(sql, Conexion.con);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                cmd.Parameters.AddWithValue("@nombre", nombreProducto);
                cmd.Parameters.AddWithValue("@descripcion", descripcionProducto);
                cmd.Parameters.AddWithValue("@precio_compra", precioCompra);
                cmd.Parameters.AddWithValue("@precio_venta", precioVenta); 
                cmd.Parameters.AddWithValue("@medida", medida);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@Categoria_idCategoria", idCategoriaProducto);
                cmd.Parameters.AddWithValue("@Proveedor_idProveedor", idProveedorProducto);
                cmd.Parameters.AddWithValue("@marca_idMarca", idMarcaProducto);
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
    }
}
