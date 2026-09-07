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
            string nombre,
            string cedula,
            string telefono,
            string correo)
        {
            return new Cliente(
                nombre,
                cedula,
                telefono,
                correo);
        }
    }
}
