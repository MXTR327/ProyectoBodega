using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CN_frmAgregarVendedor
    {
        CD_frmAgregarVendedor cd_agregarvendedor = new CD_frmAgregarVendedor();

        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public CN_frmAgregarVendedor(string usuario, string nombre, string telefono, string direccion)
        {
            this.Usuario = usuario;
            this.Nombre = nombre;
            this.Telefono = telefono;
            this.Direccion = direccion;
        }
        public bool verificarExistencia()
        {
            return cd_agregarvendedor.VerSiNoExisteVendedor(this.Usuario);
        }
        public bool AgregarVendedor()
        {
            bool rpta = false;

            rpta = cd_agregarvendedor.insertarVendedor(this.Usuario, this.Nombre, this.Telefono, this.Direccion);

            return rpta;
        }
    }
}
