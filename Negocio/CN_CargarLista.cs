using Datos;
using System.Data;

namespace Negocio
{
    public class CN_CargarLista
    {
        CD_CargarLista cd_listaproveedor = new CD_CargarLista();
        public DataTable ListarProveedor()
        {
            DataTable dt = new DataTable();
            dt = cd_listaproveedor.ConsultarProveedor();
            return dt;
        }
        public DataTable ListarCategoria()
        {
            DataTable dt = new DataTable();
            dt = cd_listaproveedor.ConsultarCategoria();
            return dt;
        }
        public DataTable ListarMarca()
        {
            DataTable dt = new DataTable();
            dt = cd_listaproveedor.ConsultarMarca();
            return dt;
        }
    }
}
