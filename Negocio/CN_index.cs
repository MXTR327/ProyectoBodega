﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CN_index
    {
        CD_index cd_index = new CD_index();
        public string idProducto { get; set; }
        public string Nombre { get; set; }
        public string Precio { get; set; }
        public string Cantidad { get; set; }
        public string StockFinal { get; set; }


        public DataTable tblProducto(string filtro)
        {
            DataTable dt = new DataTable();
            dt = cd_index.tablaProductosVendidos(filtro);
            return dt;
        }
    }
}
