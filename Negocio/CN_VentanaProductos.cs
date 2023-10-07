using Datos;
using System.Data;

namespace Negocio
{
    public class CN_VentanaProductos
    {
        CD_VentanaProductos cd_ventanaproductos = new CD_VentanaProductos();

        public string idProducto { get; set; }
        public string nombreProducto { get; set; }
        public string nombre_producto_original { get; set; }
        public string descripcionProducto { get; set; }
        public string idCategoriaProducto { get; set; }
        public string idProveedorProducto { get; set; }
        public string idMarcaProducto { get; set; }
        public string precioCompra { get; set; }
        public string precioVenta { get; set; }
        public string medida { get; set; }
        public string stock { get; set; }

        public CN_VentanaProductos(string idProducto, string nombreProducto, string descripcion, string precioCompra, string precioVenta, string medida, string stock, string idCategoria, string idProveedor, string idMarca)
        {
            this.idProducto = idProducto;
            this.nombreProducto = nombreProducto;
            descripcionProducto = descripcion;
            idCategoriaProducto = idCategoria;
            idProveedorProducto = idProveedor;
            idMarcaProducto = idMarca;
            this.precioCompra = precioCompra;
            this.precioVenta = precioVenta;
            this.medida = medida;
            this.stock = stock;
        }

        public CN_VentanaProductos()
        {
        }

        public bool ActualizarProducto()
        {
            return cd_ventanaproductos.ActualizarProductoDB(this.idProducto, this.nombreProducto, this.descripcionProducto, this.precioCompra, this.precioVenta, this.medida, this.stock, this.idCategoriaProducto, this.idProveedorProducto, this.idMarcaProducto);

        }
        public bool verificarExistencia(string nombreProducto)
        {
            return cd_ventanaproductos.VerSiNoExisteProducto(nombreProducto);
        }
        public DataTable ObtenerDatosProducto(string idProducto)
        {
            return cd_ventanaproductos.ObtenerDatos(idProducto);
        }
        public bool borrarProducto()
        {
            return cd_ventanaproductos.BorrarProductoDB(this.idProducto);
        }

        public string idProveedor { get; set; }
        public string idCategoria { get; set; }
        public string idMarca { get; set; }
        public bool borrarProveedor()
        {
            return cd_ventanaproductos.BorrarProveedorDB(this.idProveedor);
        }
        public bool borrarCategoria()
        {
            return cd_ventanaproductos.BorrarCategoriaDB(this.idCategoria);
        }
        public bool borrarMarca()
        {
            return cd_ventanaproductos.BorrarMarcaDB(this.idMarca);
        }
        //------------------------------------------------------------------------------------------------------------------------------\\
        public DataTable tblProducto(string filtro)
        {
            DataTable dt = new DataTable();
            dt = cd_ventanaproductos.tablaProductos(filtro);
            return dt;
        }
        public DataTable tblProveedor(string filtro)
        {
            DataTable dt = new DataTable();
            dt = cd_ventanaproductos.tablaProveedor(filtro);
            return dt;
        }
        public DataTable tblCategoria(string filtro)
        {
            DataTable dt = new DataTable();
            dt = cd_ventanaproductos.tablaCategoria(filtro);
            return dt;
        }
        public DataTable tblMarca(string filtro)
        {
            DataTable dt = new DataTable();
            dt = cd_ventanaproductos.tablaMarca(filtro);
            return dt;
        }
    }
}
