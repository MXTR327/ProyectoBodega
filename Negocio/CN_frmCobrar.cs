using System;
using System.Globalization;
using Datos;

namespace Negocio
{
    public class CN_frmCobrar
    {
        CD_frmCobrar cd_cobrar = new CD_frmCobrar();
        public CN_frmCobrar(int idVenta, decimal gananciaTotalProductos)
        {
            IdVenta = idVenta;
            GananciaTotalProductos = gananciaTotalProductos;
        }
        public CN_frmCobrar(int idVenta, int idProducto, int cantidadProducto, decimal precioUnitarioProducto)
        {
            IdVenta = idVenta;
            IdProducto = idProducto;
            CantidadProducto = cantidadProducto;
            PrecioUnitarioProducto = precioUnitarioProducto;
        }
        public CN_frmCobrar(string nombre_cliente, int idVendedor, decimal total_venta, decimal ganancia, decimal vuelto)
        {
            Nombre_cliente = nombre_cliente;
            IdVendedor = idVendedor;
            Total_venta = total_venta;
            Ganancia = ganancia;
            Vuelto = vuelto;
        }
        public string fechaVenta = DateTime.Now.Date.ToString("dd/MM/yyyy");
        public string horaVenta = DateTime.Now.ToString("HH:mm:ss");
        public string Nombre_cliente { get; }
        public int IdVendedor { get; }
        public decimal Total_venta { get; }
        public decimal Ganancia { get; }
        public decimal Vuelto { get; }
        public int IdVenta { get; }
        public int IdProducto { get; }
        public int CantidadProducto { get; }
        public decimal PrecioUnitarioProducto { get; }
        public decimal GananciaTotalProductos { get; }
        public int AgregarVenta()
        {
            return cd_cobrar.NuevaVenta(this.fechaVenta, this.horaVenta, this.Nombre_cliente, this.IdVendedor, this.Total_venta, this.Ganancia, this.Vuelto);
        }
        public bool ActualizarGananciaVenta()
        {
            return cd_cobrar.actualizarVenta(this.IdVenta, this.GananciaTotalProductos);
        }
        public decimal AgregarDetalleVenta()
        {
            return cd_cobrar.NuevoDetalleVenta(this.IdVenta, this.IdProducto, this.CantidadProducto, this.PrecioUnitarioProducto);
        }
    }
}
