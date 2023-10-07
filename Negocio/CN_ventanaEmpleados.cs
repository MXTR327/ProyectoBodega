using Datos;
using System;
using System.Data;

namespace Negocio
{
    public class CN_ventanaEmpleados
    {
        CD_ventanaEmpleados cd_ventanaempleados = new CD_ventanaEmpleados();

        public string idVendedor { get; set; }

        public bool borrarVendedor()
        {
            return cd_ventanaempleados.BorrarVendedorDB(this.idVendedor);
        }

        public DataTable tblEmpleados(string filtro)
        {
            DataTable dt = new DataTable();
            dt = cd_ventanaempleados.tablaProductos(filtro);
            return dt;
        }
    }
}
