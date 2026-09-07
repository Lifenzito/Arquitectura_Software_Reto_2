using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioDescuento
    {
        public decimal CalcularDescuento(
            decimal precio,
            Cliente cliente)
        {
            IConvenio? convenio =
                cliente.Convenio;

            if (convenio == null)
            {
                return 0;
            }

            return convenio.CalcularBeneficio(precio);
        }
    }
}
