using Datos;

namespace Negocio
{
    public class CN_frmAgregarProductos
    {
        CD_frmAgregarProductos cd_frmproductos = new CD_frmAgregarProductos();
        public string nombre_producto { get; set; }
        public string descripcion { get; set; }
        public decimal precio_compra { get; set; }
        public decimal precio_venta { get; set; }
        public string medida { get; set; }
        public int stock { get; set; }
        public string nombre_Categoria { get; set; }
        public string nombre_Proveedor { get; set; }
        public string nombre_Marca { get; set; }
        public CN_frmAgregarProductos()
        {
        }
        public CN_frmAgregarProductos(string nombre_producto, string descripcion, decimal precio_compra, decimal precio_venta, string medida, int stock, string nombre_Categoria, string nombre_Proveedor, string nombre_Marca)
        {
            this.nombre_producto = nombre_producto;
            this.descripcion = descripcion;
            this.precio_compra = precio_compra;
            this.precio_venta = precio_venta;
            this.medida = medida;
            this.stock = stock;
            this.nombre_Categoria = nombre_Categoria;
            this.nombre_Proveedor = nombre_Proveedor;
            this.nombre_Marca = nombre_Marca;
        }
        public bool VerificarExistencia(string valor)
        {
            return cd_frmproductos.VerSiNoExisteProducto(valor);
        }
        public bool SubirProducto()
        {
            return cd_frmproductos.SubirProductoDB(this.nombre_producto, this.descripcion, this.precio_compra, this.precio_venta, this.medida, this.stock, this.nombre_Categoria, this.nombre_Proveedor, this.nombre_Marca);
        }
    }
}
