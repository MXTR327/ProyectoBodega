using Datos;

namespace Negocio
{
    public class CN_frmAgregarProveedor
    {
        CD_frmAgregarProveedor cd_frmproveedor = new CD_frmAgregarProveedor();
        public string idProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public string direccionProveedor { get; set; }
        public string numeroContacto { get; set; }
        public CN_frmAgregarProveedor()
        {
        }
        public CN_frmAgregarProveedor(string idProv, string nombreProveedor, string direccionProveedor, string numeroContacto)
        {
            idProveedor = idProv;
            this.nombreProveedor = nombreProveedor;
            this.direccionProveedor = direccionProveedor;
            this.numeroContacto = numeroContacto;
        }
        public bool VerificarExistencia(string valor)
        {
            return cd_frmproveedor.VerSiNoExisteProveedor(valor);
        }
        public bool SubirProveedor()
        {
            return cd_frmproveedor.AgregarProveedorDB(this.nombreProveedor, this.direccionProveedor, this.numeroContacto);
        }
        public bool ActualizarProveedor()
        {
            return cd_frmproveedor.ActualizarProveedorDB(this.idProveedor, this.nombreProveedor, this.direccionProveedor, this.numeroContacto);
        }
    }
}
