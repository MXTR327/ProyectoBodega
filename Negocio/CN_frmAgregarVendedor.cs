using Datos;
using System;

namespace Negocio
{
    public class CN_frmAgregarVendedor
    {
        CD_frmAgregarVendedor cd_agregarvendedor = new CD_frmAgregarVendedor();
        public string idVendedor { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public CN_frmAgregarVendedor(string idVendedor,string usuario, string nombre, string telefono, string direccion)
        {
            this.idVendedor = idVendedor;
            this.Usuario = usuario;
            this.Nombre = nombre;
            this.Telefono = telefono;
            this.Direccion = direccion;
        }

        public CN_frmAgregarVendedor()
        {
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

        public bool ActualizarVendedor()
        {

            return cd_agregarvendedor.ActualizarVendedorDB(this.idVendedor, this.Usuario, this.Nombre, this.Telefono, this.Direccion);
        }
    }
}
