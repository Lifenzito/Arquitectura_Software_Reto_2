using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioNotificacion : IServicioNotificacion
    {
        private readonly ConsoleColor color;

        public ServicioNotificacion(ConsoleColor color)
        {
            this.color = color;
        }

        public void EnviarNotificacion(string mensaje)
        {
            Console.ForegroundColor = color;

            Console.WriteLine(mensaje);

            Console.ResetColor();
        }
    }
}
