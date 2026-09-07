using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Eventos;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioMovimiento
    {
        private readonly IMovimientoRepository movimientoRepository;

        private readonly EventoMovimiento eventoMovimiento;

        public ServicioMovimiento(
            IMovimientoRepository movimientoRepository,
            EventoMovimiento eventoMovimiento)
        {
            this.movimientoRepository = movimientoRepository;

            this.eventoMovimiento = eventoMovimiento;
        }

        public void RegistrarMovimiento(
            Movimiento movimiento)
        {
            movimientoRepository
                .RegistrarMovimiento(movimiento);

            eventoMovimiento.Disparar(
                movimiento.Tipo);
        }

        public List<Movimiento>
            ObtenerMovimientos()
        {
            return movimientoRepository
                .ObtenerMovimientos();
        }
    }
}
