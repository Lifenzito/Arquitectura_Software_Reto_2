using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Enum;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class MedicamentoCapsulaFactory : IProductoFactory
    {
        public MedicamentoCapsula Crear(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Proveedor proveedor,
            TipoRelleno tipoRelleno)
        {
            return new MedicamentoCapsula(
                nombre,
                precio,
                stock,
                stockMinimo,
                fechaVencimiento,
                (Laboratorio)proveedor,
                tipoRelleno);
        }

        public Producto Crear(
            Dictionary<string, string> datos)
        {
            Laboratorio laboratorio =
                new Laboratorio(
                    datos["proveedor"],
                    "Medellin",
                    "4444444");

            return Crear(
                datos["nombre"],
                decimal.Parse(datos["precio"]),
                int.Parse(datos["stock"]),
                int.Parse(datos["stockMinimo"]),
                DateTime.Parse(datos["fechaVencimiento"]),
                laboratorio,
                TipoRelleno.Gel);
        }
    }
}
