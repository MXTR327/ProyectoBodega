using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CN_BuscarCombobox
    {
        CD_BuscarCombobox CD_BuscarCombobox = new CD_BuscarCombobox();
        public DataTable tblProducto(object filtro, string tag)
        {
            DataTable dt = new DataTable();
            dt = CD_BuscarCombobox.tabla(filtro, tag);
            return dt;
        }
    }
}
