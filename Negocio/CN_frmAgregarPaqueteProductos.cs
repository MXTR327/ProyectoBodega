using Datos;

namespace Negocio
{
    public class CN_frmAgregarPaqueteProductos
    {
        CD_frmAgregarPaqueteProductos cd_agregarProducto = new CD_frmAgregarPaqueteProductos();
        public CN_frmAgregarPaqueteProductos(string nombreProducto, string descripcion, double precioCompraUni, double precioVentaUni, string medida, int sUnidad, string idCategoria, string idProveedor, string idMarca)
        {
            NombreProducto = nombreProducto;
            Descripcion = descripcion;
            PrecioCompraUni = precioCompraUni;
            PrecioVentaUni = precioVentaUni;
            Medida = medida;
            SUnidad = sUnidad;
            IdCategoria = idCategoria;
            IdProveedor = idProveedor;
            IdMarca = idMarca;
        }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public double PrecioCompraUni { get; set; }
        public double PrecioVentaUni { get; set; }
        public string Medida { get; set; }
        public int SUnidad { get; set; }
        public string IdCategoria { get; set; }
        public string IdProveedor { get; set; }
        public string IdMarca { get; set; }

        public bool VerificarExistencia()
        {
            return cd_agregarProducto.VerSiNoExisteProducto(this.NombreProducto);
        }
        public bool SubirProducto()
        {
            return cd_agregarProducto.SubirProductoDB(this.NombreProducto, this.Descripcion, this.PrecioCompraUni, this.PrecioVentaUni, this.Medida, this.SUnidad, this.IdCategoria, this.IdProveedor, this.IdMarca);
        }

    }
}
