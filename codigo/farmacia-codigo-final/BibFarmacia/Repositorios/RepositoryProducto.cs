using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Repositorios
{
    public class RepositoryProducto : IRepositoryProducto
    {
        // Numero de columnas del productos.txt heredado, que no trae
        // discriminador de tipo.
        private const int ColumnasSinTipo = 6;

        private const string TipoPorDefecto = "medicamento_capsula";

        private readonly List<Producto> productos;

        // Registro tipado: el discriminador selecciona a la vez la fabrica y
        // el esquema ordenado de columnas con el que se arma el diccionario.
        private readonly Dictionary<string, IProductoFactory> fabricas;
        private readonly Dictionary<string, string[]> esquemas;

        public RepositoryProducto()
        {
            productos = new List<Producto>();

            fabricas =
                new Dictionary<string, IProductoFactory>();

            esquemas =
                new Dictionary<string, string[]>();
        }

        public void RegistrarProducto(
            string tipo,
            IProductoFactory fabrica,
            string[] esquema)
        {
            if (fabricas.ContainsKey(tipo))
            {
                throw new ArgumentException(
                    $"Tipo de producto ya registrado: {tipo}");
            }

            fabricas.Add(tipo, fabrica);

            esquemas.Add(tipo, esquema);
        }

        public List<Producto> ObtenerProductos()
        {
            return productos;
        }

        public string AgregarProducto(
            Producto producto)
        {
            try
            {
                productos.Add(producto);

                return "Producto agregado";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CargarDesdeArchivo(
            string ruta)
        {
            try
            {
                if (!File.Exists(ruta))
                {
                    return "Archivo no encontrado";
                }

                string[] lineas =
                    File.ReadAllLines(ruta);

                foreach (string linea in lineas)
                {
                    string[] datos =
                        linea.Split(';');

                    string tipo =
                        datos.Length == ColumnasSinTipo
                            ? TipoPorDefecto
                            : datos[datos.Length - 1];

                    IProductoFactory fabrica =
                        fabricas[tipo];

                    string[] esquema =
                        esquemas[tipo];

                    Dictionary<string, string> valores =
                        new Dictionary<string, string>();

                    for (int columna = 0;
                        columna < esquema.Length &&
                        columna < datos.Length;
                        columna++)
                    {
                        valores[esquema[columna]] =
                            datos[columna];
                    }

                    productos.Add(
                        fabrica.Crear(valores));
                }

                return "Productos cargados";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
