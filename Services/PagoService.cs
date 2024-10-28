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

        //baja
        public async Task EliminarPagoAsync(int idPagoEliminar)
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

        //listas
        public async Task<IEnumerable<Pago>> ObtenerPagosAsync() => await _pagoRepository.ReturnListaAsync();

        public async Task<IEnumerable<Pago>> ObtenerPagosPorIdClienteAsync(int idCliente)
        {
            var cliente = await _clienteRepository.EncontrarPorIDAsync(idCliente);

            if (cliente == null)
            {
                throw new KeyNotFoundException("El cliente con el Id especificado no existe.");
            }

            return await _pagoRepository.ObtenerPagoPorClienteAsync(idCliente);
        }

        public async Task<IEnumerable<Pago>> ObtenerPagosPorDniClienteAsync(string dniCliente)
        {
            var cliente = await _clienteRepository.ObtenerClienteConDniAsync(dniCliente);
            if (cliente == null)
            {
                throw new KeyNotFoundException("El cliente con el DNI especificado no existe.");
            }

            return await _pagoRepository.ObtenerPagoPorClienteAsync(cliente.Id);

        }


        public async Task<Pago> BuscarPagoPorIdAsync(int id) => await _pagoRepository.EncontrarPorIDAsync(id);


        public async Task<IEnumerable<Pago>> FiltrarPagosPorPropiedadAsync(string propiedad, string prefijo)
        {
            string trimedPrefijo = prefijo.Trim();
            string operador = Utils.ObtenerOperador(trimedPrefijo);
            switch (propiedad.ToLower())
            {
                case "id":
                    string valorID = trimedPrefijo.TrimStart('>', '<', '=', ' ');
                    if (int.TryParse(valorID, out int resultadoInt))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.Id >= resultadoInt);
                            case "<=":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.Id <= resultadoInt);
                            case ">":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.Id > resultadoInt);
                            case "<":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.Id < resultadoInt);
                            case "=":
                            default:
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.Id == resultadoInt);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de ID no valido.");
                    }
                case "idcliente":
                    string valorIDCliente = trimedPrefijo.TrimStart('>', '<', '=', ' ');
                    if (int.TryParse(valorIDCliente, out int resultadoInt2))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdCliente >= resultadoInt2);
                            case "<=":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdCliente <= resultadoInt2);
                            case ">":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdCliente > resultadoInt2);
                            case "<":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdCliente < resultadoInt2);
                            case "=":
                            default:
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdCliente == resultadoInt2);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de ID Cliente no valido.");
                    }
                case "idmembresia":
                    string valorIDMembresia = trimedPrefijo.TrimStart('>', '<', '=', ' ');
                    if (int.TryParse(valorIDMembresia, out int resultadoInt3))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdMembresia >= resultadoInt3);
                            case "<=":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdMembresia <= resultadoInt3);
                            case ">":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdMembresia > resultadoInt3);
                            case "<":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdMembresia < resultadoInt3);
                            case "=":
                            default:
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.IdMembresia == resultadoInt3);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de ID Cliente no valido.");
                    }
                case "fechapago":

                    string valorFecha = trimedPrefijo.TrimStart('>', '<', '=', ' ');

                    if (DateTime.TryParse(valorFecha, out DateTime fecha))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.FechaPago >= fecha);
                            case "<=":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.FechaPago <= fecha);
                            case ">":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.FechaPago > fecha);
                            case "<":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.FechaPago < fecha);
                            case "=":
                            default:
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.FechaPago == fecha);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de fecha no valido.");
                    }
                case "monto":
                    string valorMonto = trimedPrefijo.TrimStart('>', '<', '=', ' ');
                    if (double.TryParse(valorMonto, out double resultadoDouble))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.Monto >= resultadoDouble);
                            case "<=":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.Monto <= resultadoDouble);
                            case ">":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.Monto > resultadoDouble);
                            case "<":
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.Monto < resultadoDouble);
                            case "=":
                            default:
                                return await _pagoRepository.EncontrarPorCondicionAsync(p => p.Monto == resultadoDouble);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de monto no valido.");
                    }
                default:
                    throw new ArgumentOutOfRangeException($"La clase Pago no contiene la propiedad {propiedad}.");
            }

        }
    }
}
