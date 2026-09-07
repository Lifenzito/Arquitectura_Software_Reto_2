using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Verificadores
{
    public class VerificadorStock : IVerificador
    {
        private readonly IEvento evento;

        public VerificadorStock(
            IEvento evento)
        {
            this.evento = evento;
        }

        public void Verificar(
            Producto producto)
        {
            if (producto is not IProductoConStock
                productoConStock)
            {
                return;
            }

            if (productoConStock.Stock <=
                productoConStock.StockMinimo)
            {
                evento.Disparar(producto);
            }
        }
    }
}
