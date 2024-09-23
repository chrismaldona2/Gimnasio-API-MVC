using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Services.Contratos;
using Data.Repositorios;
using Data.Repositorios.Contratos;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepositorio _pagoRepository;
        private readonly IClienteRepositorio _clienteRepository;
        private readonly IMembresiaRepositorio _membresiaRepository;

        public PagoService(IPagoRepositorio pagoRepository, IClienteRepositorio clienteRepository, IMembresiaRepositorio membresiaRepository)
        {
            _pagoRepository = pagoRepository;
            _clienteRepository = clienteRepository;
            _membresiaRepository = membresiaRepository;
        }

        //alta
        public async Task RegistrarPagoAsync(int idCliente, int idMembresia)
        {

            var cliente = await _clienteRepository.EncontrarPorIDAsync(idCliente);
            if (cliente == null)
            {
                throw new KeyNotFoundException("La cliente especificado no fue encontrado.");
            }

            var membresia = await _membresiaRepository.EncontrarPorIDAsync(idMembresia);
            if (membresia == null)
            {
                throw new KeyNotFoundException("La membresía especificada no fue encontrada.");
            }

            var pago = new Pago(idCliente, idMembresia, membresia.Precio);

            try
            {
                await _pagoRepository.CrearAsync(pago);

                cliente.IdMembresia = pago.IdMembresia;

                if (cliente.FechaVencimientoMembresia == null || cliente.FechaVencimientoMembresia < DateTime.Now)
                {
                    // Si la fecha de vencimiento es null o pasada, establecer la nueva fecha de vencimiento
                    cliente.FechaVencimientoMembresia = DateTime.Now.AddDays(membresia.DuracionDias);
                }
                else
                {
                    // Si la fecha de vencimiento es válida y futura, extender la fecha de vencimiento
                    cliente.FechaVencimientoMembresia = cliente.FechaVencimientoMembresia.Value.AddDays(membresia.DuracionDias);
                }

                await _clienteRepository.ModificarAsync(cliente);
                await _pagoRepository.GuardarCambiosAsync();
                await _clienteRepository.GuardarCambiosAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar registrar el pago.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }

        //baja
        public async Task EliminarPagoAsync(int idPagoEliminar)
        {
            try
            {
                var pagoExistente = await _pagoRepository.EncontrarPorIDAsync(idPagoEliminar);
                if (pagoExistente == null)
                {
                    throw new KeyNotFoundException("El pago con el Id especificado no existe.");
                }

                var cliente = await _clienteRepository.EncontrarPorIDAsync(pagoExistente.IdCliente);
                var membresia = await _membresiaRepository.EncontrarPorIDAsync(pagoExistente.IdMembresia);

                await _pagoRepository.EliminarAsync(idPagoEliminar);
                await _pagoRepository.GuardarCambiosAsync();


                if (cliente != null && membresia != null && cliente.FechaVencimientoMembresia.HasValue)
                {
                    cliente.FechaVencimientoMembresia = cliente.FechaVencimientoMembresia.Value.AddDays(-(membresia.DuracionDias));
                    await _clienteRepository.ModificarAsync(cliente);
                    await _clienteRepository.GuardarCambiosAsync();
                }
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar eliminar el cliente.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }

        //listas
        public async Task<IEnumerable<Pago>> ObtenerPagosAsync()
        {
            try
            {
                return await _pagoRepository.ReturnListaAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar retornar la lista de pagos.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }


        public async Task<IEnumerable<Pago>> ObtenerPagosPorIdClienteAsync(int idCliente)
        {

            try
            {
                var cliente = await _clienteRepository.EncontrarPorIDAsync(idCliente);

                if (cliente == null)
                {
                    throw new KeyNotFoundException("El cliente con el Id especificado no existe.");
                }

                return await _pagoRepository.ObtenerPagoPorClienteAsync(idCliente);
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar retornar la lista de asistencias del cliente.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }

        public async Task<IEnumerable<Pago>> ObtenerPagosPorDniClienteAsync(string dniCliente)
        {

            try
            {
                var cliente = await _clienteRepository.ObtenerClienteConDniAsync(dniCliente);
                if (cliente == null)
                {
                    throw new KeyNotFoundException("El cliente con el DNI especificado no existe.");
                }

                return await _pagoRepository.ObtenerPagoPorClienteAsync(cliente.Id);

            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Se produjo un error al intentar retornar la lista de asistencias del cliente.");
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error inesperado al intentar realizar la acción:", ex);
            }
        }
    }
}
