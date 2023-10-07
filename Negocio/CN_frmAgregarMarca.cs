using Datos;

namespace Negocio
{
    public class CN_frmAgregarMarca
    {
        CD_frmAgregarMarca cd_frmmarca = new CD_frmAgregarMarca();
        public string idMarca { get; set; }
        public string nombreMarca { get; set; }
        public CN_frmAgregarMarca()
        {

        }
        public CN_frmAgregarMarca(string idMarca, string nombre_marca)
        {
            this.idMarca = idMarca;
            this.nombreMarca = nombre_marca;
        }
        public bool verificarExistencia(string nombre)
        {
            return cd_frmmarca.VerSiNoExisteMarca(nombre);
        }

        public bool SubirMarca()
        {
            return cd_frmmarca.AgregarMarcaDB(this.nombreMarca);
        }

        public bool ActualizarMarca()
        {
            return cd_frmmarca.ActualizarMarcaDB(this.idMarca, this.nombreMarca);
        }
    }
}
