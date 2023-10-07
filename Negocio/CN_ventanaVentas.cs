using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CN_ventanaVentas
    {
        CD_ventanaVentas cd_ventanventas = new CD_ventanaVentas();

        public DataTable tblVentas(string fechaCompleta)
        {
            DataTable dt = new DataTable();
            dt = cd_ventanventas.tablaVentas(fechaCompleta);
            return dt;
        }
        public DataTable tblDetalleVenta(string idVenta)
        {
            DataTable dt = new DataTable();
            dt = cd_ventanventas.tablaDetalleVentas(idVenta);
            return dt;
        }
        public DataTable tblDetalleProducto(string idProducto)
        {
            DataTable dt = new DataTable();
            dt = cd_ventanventas.detalleProducto(idProducto);
            return dt;
        }
    }
}
