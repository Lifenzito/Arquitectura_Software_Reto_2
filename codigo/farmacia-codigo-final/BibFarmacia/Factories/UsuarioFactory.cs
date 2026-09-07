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
            Dictionary<string, string> datos)
        {
            return new Usuario(
                datos["nombre"],
                datos["cedula"],
                datos["telefono"],
                datos["correo"],
                datos["usuario"],
                datos["contrasena"]);
        }
    }
}
