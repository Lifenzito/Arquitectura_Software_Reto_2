using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class ClienteFactory : IClienteFactory
    {
        public Cliente Crear(
            Dictionary<string, string> datos)
        {
            return new Cliente(
                datos["nombre"],
                datos["cedula"],
                datos["telefono"],
                datos["correo"]);
        }
    }
}
