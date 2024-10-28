using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Contratos;
using Data.Repositorios;
using Data.Repositorios.Contratos;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;

namespace Services
{
    public class MembresiaService : IMembresiaService
    {
        private readonly IMembresiaRepositorio _membresiaRepository;
        private readonly IClienteRepositorio _clienteRepository;

        public MembresiaService(IMembresiaRepositorio membresiaRepository, IClienteRepositorio clienteRepository)
        {
            _membresiaRepository = membresiaRepository;
            _clienteRepository = clienteRepository;
        }

        //alta
        public async Task RegistrarMembresiaAsync(string tipo, int duraciondias, double precio)
        {
            if (string.IsNullOrWhiteSpace(tipo))
            {
                throw new ArgumentNullException("El tipo de membresía no puede ser nulo.");
            }
            if (precio < 0)
            {
                throw new ArgumentException("El precio debe ser un número mayor o igual a cero.");
            }

            if (duraciondias <= 0)
            {
                throw new ArgumentException("La duración debe ser mayor a 0 días.");
            }

            var membresia = new Membresia(tipo.Trim(), duraciondias, precio);
            
            await _membresiaRepository.CrearAsync(membresia);
            await _membresiaRepository.GuardarCambiosAsync();

        }

        //baja
        public async Task EliminarMembresiaAsync(int idMembresiaEliminar)
        {

            var membresiaExistente = await _membresiaRepository.EncontrarPorIDAsync(idMembresiaEliminar);
            if (membresiaExistente == null)
            {
                throw new KeyNotFoundException("La membresía con el Id especificado no existe.");
            }

            var clientesAsociados = await _clienteRepository.EncontrarPorCondicionAsync(c => c.IdMembresia == idMembresiaEliminar);
            if (clientesAsociados.Any())
            {
                throw new InvalidOperationException("No se puede eliminar la membresía porque tiene clientes asociados.");
            }

            await _membresiaRepository.EliminarAsync(idMembresiaEliminar);
            await _membresiaRepository.GuardarCambiosAsync();

        }

        //modificacion
        public async Task ModificarMembresiaAsync(Membresia membresiaModificar)
        {
            if (membresiaModificar == null)
            {
                throw new ArgumentNullException("La membresía a modificar no puede ser nula.");
            }

            if (string.IsNullOrWhiteSpace(membresiaModificar.Tipo))
            {
                throw new ArgumentNullException("El tipo de membresía no puede ser nulo.");
            }

            if (membresiaModificar.Precio < 0)
            {
                throw new ArgumentException("El precio debe ser un número mayor o igual a cero.");
            }

            if (membresiaModificar.DuracionDias <= 0)
            {
                throw new ArgumentException("La duración debe ser mayor a 0 días.");
            }


            var membresiaExistente = await _membresiaRepository.EncontrarPorIDAsync(membresiaModificar.Id);
            if (membresiaExistente == null)
            {
                throw new KeyNotFoundException("La membresía con el Id especificado no existe.");
            }

            membresiaExistente.Tipo = membresiaModificar.Tipo.Trim();
            membresiaExistente.Precio = membresiaModificar.Precio;
            membresiaExistente.DuracionDias = membresiaModificar.DuracionDias;

            await _membresiaRepository.ModificarAsync(membresiaExistente);
            await _membresiaRepository.GuardarCambiosAsync();

        }

        //lista
        public async Task<IEnumerable<Membresia>> ObtenerMembresiasAsync() => await _membresiaRepository.ReturnListaAsync();


        public async Task<Membresia> BuscarMembresiaPorIdAsync(int id) => await _membresiaRepository.EncontrarPorIDAsync(id);



        public async Task<IEnumerable<Membresia>> FiltrarMembresiasPorPropiedadAsync(string propiedad, string prefijo)
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
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Id >= resultadoInt);
                            case "<=":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Id <= resultadoInt);
                            case ">":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Id > resultadoInt);
                            case "<":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Id < resultadoInt);
                            case "=":
                            default:
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Id == resultadoInt);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de ID no valido.");
                    }
                case "tipo":
                    return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Tipo.StartsWith(trimedPrefijo));
                case "duraciondias":
                    string valorDias = trimedPrefijo.TrimStart('>', '<', '=', ' ');
                    if (int.TryParse(valorDias, out int resultadoInt2))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.DuracionDias >= resultadoInt2);
                            case "<=":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.DuracionDias <= resultadoInt2);
                            case ">":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.DuracionDias > resultadoInt2);
                            case "<":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.DuracionDias < resultadoInt2);
                            case "=":
                            default:
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.DuracionDias == resultadoInt2);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de duración de días no valido.");
                    }

                case "precio":
                    
                    string valorPrecio = trimedPrefijo.TrimStart('>', '<', '=', ' ');
                    if (double.TryParse(valorPrecio, out double resultadoDouble))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Precio >= resultadoDouble);
                            case "<=":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Precio <= resultadoDouble);
                            case ">":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Precio > resultadoDouble);
                            case "<":
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Precio < resultadoDouble);
                            case "=":
                            default:
                                return await _membresiaRepository.EncontrarPorCondicionAsync(m => m.Precio == resultadoDouble);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de monto no valido.");
                    }
                default:
                    throw new ArgumentOutOfRangeException($"La clase Membresía no contiene la propiedad {propiedad}.");
            }

        }
    }
}

