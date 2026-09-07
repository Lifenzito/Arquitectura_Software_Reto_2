using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Enum;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Convenios
{
    public class ConvenioEmpresa : IConvenio
    {
        private readonly decimal porcentaje;

        public string NombreEntidad { get; }

        public string TipoEntidad => "Empresa";

        public TipoBeneficio TipoBeneficio =>
            TipoBeneficio.Descuento;

        public ConvenioEmpresa(
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
    }
}
