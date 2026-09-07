using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class UsuarioFactory : IUsuarioFactory
    {
        public Usuario Crear(
            string nombre,
            string cedula,
            string telefono,
            string correo,
            string usuario,
            string contrasena)
        {
            return new Usuario(
                nombre,
                cedula,
                telefono,
                correo,
                usuario,
                contrasena);
        }
    }
}
