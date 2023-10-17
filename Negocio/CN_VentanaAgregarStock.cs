using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CN_VentanaAgregarStock
    {
        CD_VentanaAgregarStock cd_ventanaagregarstock = new CD_VentanaAgregarStock();

        public int idProducto { get; set; }
        public string cantidadAgregar { get; set; }

        public bool AgregarCantidad()
        {
            return cd_ventanaagregarstock.AgregarCantidadProductoDB(idProducto, cantidadAgregar);
        }


        public DataTable tblProducto(string filtro)
        {
            DataTable dt = new DataTable();
            dt = cd_ventanaagregarstock.tablaProductos(filtro);
            return dt;
        }
    }
}
