using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CN_frmCobrar
    {
        CD_frmCobrar cd_cobrar = new CD_frmCobrar();
        public CN_frmCobrar(int idVenta, double gananciaTotalProductos)
        {
            IdVenta = idVenta;
            GananciaTotalProductos = gananciaTotalProductos;
        }

        public CN_frmCobrar(int idVenta, int idProducto, int cantidadProducto, double precioUnitarioProducto)
        {
            IdVenta = idVenta;
            IdProducto = idProducto;
            CantidadProducto = cantidadProducto;
            PrecioUnitarioProducto = precioUnitarioProducto;
        }

        public CN_frmCobrar(string nombre_cliente, int idVendedor, double total_venta, double ganancia, double vuelto)
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
        public double Total_venta { get; }
        public double Ganancia { get; }
        public double Vuelto { get; }
        public int IdVenta { get; }
        public int IdProducto { get; }
        public int CantidadProducto { get; }
        public double PrecioUnitarioProducto { get; }
        public double GananciaTotalProductos { get; }

        public int AgregarVenta()
        {
            return cd_cobrar.NuevaVenta(this.fechaVenta, this.horaVenta, this.Nombre_cliente, this.IdVendedor, this.Total_venta, this.Ganancia, this.Vuelto);
        }
        public bool ActualizarGananciaVenta()
        {
            return cd_cobrar.actualizarVenta(this.IdVenta, this.GananciaTotalProductos);
        }

        public double AgregarDetalleVenta()
        {
            return cd_cobrar.NuevoDetalleVenta(this.IdVenta, this.IdProducto, this.CantidadProducto, this.PrecioUnitarioProducto);
        }
    }
}
