using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class ComestibleFactory : IProductoFactory
    {
        public Comestible Crear(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Proveedor proveedor)
        {
            return new Comestible(
                nombre,
                precio,
                stock,
                stockMinimo,
                fechaVencimiento,
                proveedor);
        }

        public Producto Crear(
            Dictionary<string, string> datos)
        {
            Marca marca =
                new Marca(
                    datos["proveedor"],
                    "Medellin",
                    "4444444");

            return Crear(
                datos["nombre"],
                decimal.Parse(datos["precio"]),
                int.Parse(datos["stock"]),
                int.Parse(datos["stockMinimo"]),
                DateTime.Parse(datos["fechaVencimiento"]),
                marca);
        }
    }
}
