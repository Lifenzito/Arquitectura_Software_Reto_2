using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Repositorios
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly List<Cliente> clientes;
        private readonly IClienteFactory clienteFactory;

        public ClienteRepository(
            IClienteFactory clienteFactory)
        {
            clientes = new List<Cliente>();

            this.clienteFactory = clienteFactory;
        }

        public void AgregarCliente(
            Cliente cliente)
        {
            clientes.Add(cliente);
        }

        public List<Cliente> ObtenerClientes()
        {
            return clientes;
        }

        public void AcumularPuntos(
            Cliente cliente,
            int puntos)
        {
            cliente.Puntos += puntos;
        }

        public string Cargar(
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

                    Cliente cliente =
                        clienteFactory.Crear(
                            new Dictionary<string, string>
                            {
                                ["nombre"] = datos[0],
                                ["cedula"] = datos[1],
                                ["telefono"] = datos[2],
                                ["correo"] = datos[3]
                            });

                    clientes.Add(cliente);
                }

                return "Clientes cargados";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
