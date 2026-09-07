using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Enum;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Convenios
{
    public class ConvenioMutual : IConvenio
    {
        private readonly decimal porcentaje;

        public string NombreEntidad { get; }

        public TipoBeneficio TipoBeneficio =>
            TipoBeneficio.Descuento;

        public ConvenioMutual(
            string nombreEntidad,
            decimal porcentaje)
        {
            NombreEntidad = nombreEntidad;

            this.porcentaje = porcentaje;
        }

        public decimal CalcularBeneficio(
            decimal precio)
        {
            return precio * porcentaje / 100m;
        }

        // Etiqueta con la que el convenio se identifica en pantalla.
        public override string ToString()
        {
            return $"{NombreEntidad} (Mutual)";
        }
    }
}
