using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Verificadores
{
    public class VerificadorVencimiento : IVerificador
    {
        private readonly IEvento evento;

        public VerificadorVencimiento(
            IEvento evento)
        {
            this.evento = evento;
        }

        public void Verificar(
            Producto producto)
        {
            if (producto is not IVencimiento
                productoConVencimiento)
            {
                return;
            }

            int dias =
                (productoConVencimiento.FechaVencimiento -
                DateTime.Now).Days;

            if (dias <= 30)
            {
                evento.Disparar(producto);
            }
        }
    }
}
