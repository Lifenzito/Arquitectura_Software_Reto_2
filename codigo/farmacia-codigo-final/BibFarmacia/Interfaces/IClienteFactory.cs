using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;

namespace BibFarmacia.Interfaces
{
    public interface IClienteFactory
    {
        Cliente Crear(
            string nombre,
            string cedula,
            string telefono,
            string correo);
    }
}
