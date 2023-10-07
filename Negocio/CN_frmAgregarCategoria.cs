using Datos;

namespace Negocio
{
    public class CN_frmAgregarCategoria
    {
        CD_frmAgregarCategoria cd_frmcategoria = new CD_frmAgregarCategoria();
        public string idCategoria { get; set; }
        public string nombreCategoria { get; set; }
        public string descripcionCategoria { get; set; }
        public CN_frmAgregarCategoria()
        {
        }
        public CN_frmAgregarCategoria(string idCat, string nombre_categoria, string descripcion_categoria)
        {
            idCategoria = idCat;
            this.nombreCategoria = nombre_categoria;
            this.descripcionCategoria = descripcion_categoria;
        }
        public bool VerificarExistencia(string valor)
        {
            return cd_frmcategoria.VerSiNoExisteCategoria(valor);
        }
        public bool SubirCategoria()
        {
            return cd_frmcategoria.AgregarCategoriaDB(this.nombreCategoria, this.descripcionCategoria);
        }
        public bool ActualizarCategoria()
        {
            return cd_frmcategoria.ActualizarCategoriaDB(this.idCategoria, this.nombreCategoria, this.descripcionCategoria);
        }
    }
}
