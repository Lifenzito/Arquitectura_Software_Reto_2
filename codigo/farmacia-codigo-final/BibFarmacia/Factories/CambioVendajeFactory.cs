using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class CambioVendajeFactory : IProductoFactory
    {
        public CambioVendaje Crear(
            string nombre,
            decimal precio,
            Proveedor proveedor,
            int duracionMinutos)
        {
            return new CambioVendaje(
                nombre,
                precio,
                proveedor,
                duracionMinutos);
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
                marca,
                int.Parse(datos["duracionMinutos"]));
        }
    }
}
