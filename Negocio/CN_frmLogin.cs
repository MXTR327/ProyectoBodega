using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
