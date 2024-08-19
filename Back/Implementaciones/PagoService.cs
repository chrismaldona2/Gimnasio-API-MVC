using Back.Entidades;
using Back.Implementaciones.Contratos;
using Back.Repositorios.Contratos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Back.Implementaciones
{
    public class PagoService : IPagoService
    {
        private readonly IRepositorio<Pago> _pagoRepository;
        private readonly IRepositorio<Cliente> _clienteRepository;
        private readonly IRepositorio<Membresia> _membresiaRepository;

        public PagoService(IRepositorio<Pago> pagoRepository, IRepositorio<Cliente> clienteRepository, IRepositorio<Membresia> membresiaRepository)
        {
            _pagoRepository = pagoRepository;
            _clienteRepository = clienteRepository;
            _membresiaRepository = membresiaRepository;
        }

        //PAGOS POR CLIENTE
        public async Task<IEnumerable<Pago>> ObtenerPagosPorClienteAsync(int idCliente)
        {
            var cliente = await _clienteRepository.EncontrarPorIDAsync(idCliente);

            if (cliente == null)
            {
                throw new ArgumentException("Cliente no encontrado.");
            }
            return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdCliente == idCliente).ToListAsync();
        }

        //ALTA
        public async Task RegistrarPagoAsync(int idCliente, int idMembresia, bool actualizarVencimientoCliente)
        {
            var cliente = await _clienteRepository.EncontrarPorIDAsync(idCliente);
            if (cliente == null)
            {
                throw new Exception("Cliente no encontrado.");
            }

            var membresia = await _membresiaRepository.EncontrarPorIDAsync(idMembresia);
            if (membresia == null)
            {
                throw new Exception("Membresía no encontrada.");
            }

            //se registra el pago
            var pagoNuevo = new Pago(cliente.Id, membresia.Id, membresia.Precio);
            await _pagoRepository.CrearAsync(pagoNuevo);

            //se actualiza el vencimiento de la membresia al cliente
            if (actualizarVencimientoCliente)
            {
                DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);

                if (cliente.VencimientoMembresia >= hoy)
                {
                    cliente.VencimientoMembresia = cliente.VencimientoMembresia.AddDays(membresia.DuracionEnDias);
                } else
                {
                    cliente.VencimientoMembresia = hoy.AddDays(membresia.DuracionEnDias);
                }
                
                _clienteRepository.ModificarAsync(cliente);
            }

            await _pagoRepository.GuardarCambiosAsync();
            await _clienteRepository.GuardarCambiosAsync();
        }

        // BAJA
        public async Task EliminarPagoAsync(int idPagoEliminar)
        {
            await _pagoRepository.EliminarAsync(idPagoEliminar);
            await _pagoRepository.GuardarCambiosAsync();
        }
    }
}
