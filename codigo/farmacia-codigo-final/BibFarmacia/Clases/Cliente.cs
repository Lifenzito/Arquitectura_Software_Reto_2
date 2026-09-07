using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class Cliente : Persona
    {
        public int Puntos { get; set; }
        public IConvenio? Convenio { get; set; }

        public Cliente(string nombre, string cedula,
            string telefono, string correo)
            : base(nombre, cedula, telefono, correo)
        {
            Puntos = 0;
        }

        public void AcumularPuntos(int puntos)
        {
            Puntos += puntos;
        }
    }
}
