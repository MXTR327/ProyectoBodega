using Datos;
using System.Data;

namespace Negocio
{
    public class CN_frmLogin
    {
        CD_frmLogin cd_frmlogin = new CD_frmLogin();
        public string Usuario { get; set; }

        public DataTable VerificarUsuario()
        {
            return cd_frmlogin.VerificarExistencia(this.Usuario);
        }
    }
}
