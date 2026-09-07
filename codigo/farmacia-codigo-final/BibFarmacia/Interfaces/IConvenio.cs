using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Enum;

namespace BibFarmacia.Interfaces
{
    public interface IConvenio
    {
        string NombreEntidad { get; }

        string TipoEntidad { get; }

        TipoBeneficio TipoBeneficio { get; }

        decimal CalcularBeneficio(decimal precio);
    }
}
