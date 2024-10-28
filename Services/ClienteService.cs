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
using System.Net;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepositorio _clienteRepository;

        public ClienteService(IClienteRepositorio clienteRepository)
        {
            _clienteRepository = clienteRepository;

        }

        //alta
        public async Task RegistrarClienteAsync(string dni, string nombre, string apellido, string email, string telefono, DateOnly fechaNacimiento, Sexo sexo)
        {
            Regex ValidacionNombre = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(\s[A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$");

            if (!ValidacionNombre.IsMatch(nombre.Trim())|| !ValidacionNombre.IsMatch(apellido.Trim()))
            {
                throw new NombreInvalidoException("El nombre no debe contener números ni caracteres especiales.");
            }

            Regex ValidacionEmail = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!ValidacionEmail.IsMatch(email.Trim()) || email.Split('@')[1].Contains(".."))
            {
                throw new EmailInvalidoException();
            }


            if (fechaNacimiento > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new FechaNacimientoException();
            }

            if (await _clienteRepository.ObtenerClienteConDniAsync(dni) != null)
            {
                throw new DniRegistradoException();
            }

            var cliente = new Cliente(dni.Trim(), nombre.Trim(), apellido.Trim(), email.Trim(), telefono.Trim(), fechaNacimiento, sexo, DateOnly.FromDateTime(DateTime.Now));

            await _clienteRepository.CrearAsync(cliente);
            await _clienteRepository.GuardarCambiosAsync();

        }

        //baja
        public async Task EliminarClienteAsync(int idClienteEliminar)
        {

            var clienteExistente = await _clienteRepository.EncontrarPorIDAsync(idClienteEliminar);
            if (clienteExistente == null)
            {
                throw new KeyNotFoundException("El cliente con el Id especificado no existe.");
            }
            await _clienteRepository.EliminarAsync(idClienteEliminar);
            await _clienteRepository.GuardarCambiosAsync();


        }

        //modificacion
        public async Task ModificarClienteAsync(Cliente clienteModificar)
        {
            if (clienteModificar == null)
            {
                throw new ArgumentNullException("El cliente a modificar no puede ser nulo.");
            }

            Regex ValidacionNombre = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(\s[A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$");

            if (!ValidacionNombre.IsMatch(clienteModificar.Nombre.Trim()) || !ValidacionNombre.IsMatch(clienteModificar.Apellido.Trim()))
            {
                throw new NombreInvalidoException("El nombre no debe contener números ni caracteres especiales.");
            }

            Regex ValidacionEmail = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!ValidacionEmail.IsMatch(clienteModificar.Email.Trim()) || clienteModificar.Email.Split('@')[1].Contains(".."))
            {
                throw new EmailInvalidoException();
            }

            if (clienteModificar.FechaNacimiento > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new FechaNacimientoException();
            }

            var dniUsado = await _clienteRepository.ObtenerClienteConDniAsync(clienteModificar.Dni);

            if (dniUsado != null && dniUsado.Id != clienteModificar.Id)
            {
                throw new DniRegistradoException();
            };


            var clienteExistente = await _clienteRepository.EncontrarPorIDAsync(clienteModificar.Id);
            if (clienteExistente == null)
            {
                throw new KeyNotFoundException("El cliente con el Id especificado no existe.");
            }

            clienteExistente.Dni = clienteModificar.Dni.Trim();
            clienteExistente.Nombre = clienteModificar.Nombre.Trim();
            clienteExistente.Apellido = clienteModificar.Apellido.Trim();
            clienteExistente.Email = clienteModificar.Email.Trim();
            clienteExistente.Telefono = clienteModificar.Telefono.Trim();
            clienteExistente.FechaNacimiento = clienteModificar.FechaNacimiento;
            clienteExistente.Sexo = clienteModificar.Sexo;

            await _clienteRepository.ModificarAsync(clienteExistente);
            await _clienteRepository.GuardarCambiosAsync();

        }

        //listas
        public async Task<IEnumerable<Cliente>> ObtenerClientesAsync() => await _clienteRepository.ReturnListaAsync();

        public async Task<IEnumerable<Cliente>> ObtenerClientesConMembresiaVencidaAsync() => await _clienteRepository.ObtenerClientesConMembresiaVencidaAsync();


        //buscar por dni
        public async Task<Cliente> BuscarClientePorDniAsync(string dni) => await _clienteRepository.ObtenerClienteConDniAsync(dni);

        public async Task<Cliente> BuscarClientePorIdAsync(int id) => await _clienteRepository.EncontrarPorIDAsync(id);

        public async Task<IEnumerable<Cliente>> FiltrarClientesPorPropiedadAsync(string propiedad, string prefijo)
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
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Id >= resultadoInt);
                            case "<=":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Id <= resultadoInt);
                            case ">":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Id > resultadoInt);
                            case "<":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Id < resultadoInt);
                            case "=":
                            default:
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Id == resultadoInt);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de ID no valido.");
                    }
                case "nombre":
                    return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Nombre.StartsWith(trimedPrefijo));
                case "apellido":
                    return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Apellido.StartsWith(trimedPrefijo));
                case "dni":
                    return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Dni.StartsWith(trimedPrefijo));
                case "email":
                    return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Email.StartsWith(trimedPrefijo));
                case "telefono":
                    return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Telefono.StartsWith(trimedPrefijo));
                case "sexo":
                    if (Enum.TryParse<Sexo>(trimedPrefijo, true, out var sexoValor))
                    {
                        return await _clienteRepository.EncontrarPorCondicionAsync(c => c.Sexo == sexoValor);
                    }
                    else
                    {
                        throw new FormatException("Tipo de 'sexo' no valido.");
                    }
                case "fechanacimiento":

                    string valorFecha = trimedPrefijo.TrimStart('>', '<', '=', ' ');

                    if (DateOnly.TryParse(valorFecha, out DateOnly fecha))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.FechaNacimiento >= fecha);
                            case "<=":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.FechaNacimiento <= fecha);
                            case ">":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.FechaNacimiento > fecha);
                            case "<":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.FechaNacimiento < fecha);
                            case "=":
                            default:
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.FechaNacimiento == fecha);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de fecha no valido.");
                    } 

                case "idmembresia":
                    string valorIDMembresia = trimedPrefijo.TrimStart('>', '<', '=', ' ');
                    if (int.TryParse(valorIDMembresia, out int resultadoInt2))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.IdMembresia >= resultadoInt2);
                            case "<=":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.IdMembresia <= resultadoInt2);
                            case ">":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.IdMembresia > resultadoInt2);
                            case "<":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.IdMembresia < resultadoInt2);
                            case "=":
                            default:
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.IdMembresia == resultadoInt2);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de ID Membresía no valido.");
                    }
                case "fechavencimientomembresia":
                    string valorFecha2 = trimedPrefijo.TrimStart('>', '<', '=', ' ');

                    if (DateTime.TryParse(valorFecha2, out DateTime fecha2))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c =>
                                    c.FechaVencimientoMembresia.HasValue && c.FechaVencimientoMembresia.Value >= fecha2);

                            case "<=":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c =>
                                    c.FechaVencimientoMembresia.HasValue && c.FechaVencimientoMembresia.Value <= fecha2);

                            case ">":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c =>
                                    c.FechaVencimientoMembresia.HasValue && c.FechaVencimientoMembresia.Value > fecha2);

                            case "<":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c =>
                                    c.FechaVencimientoMembresia.HasValue && c.FechaVencimientoMembresia.Value < fecha2);

                            case "=":
                            default:
                                return await _clienteRepository.EncontrarPorCondicionAsync(c =>
                                    c.FechaVencimientoMembresia.HasValue && c.FechaVencimientoMembresia.Value == fecha2);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de fecha no valido.");
                    }
                case "fecharegistro":
                    string valorFecha3 = trimedPrefijo.TrimStart('>', '<', '=', ' ');

                    if (DateOnly.TryParse(valorFecha3, out DateOnly fecha3))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.FechaRegistro >= fecha3);
                            case "<=":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.FechaRegistro <= fecha3);
                            case ">":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.FechaRegistro > fecha3);
                            case "<":
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.FechaRegistro < fecha3);
                            case "=":
                            default:
                                return await _clienteRepository.EncontrarPorCondicionAsync(c => c.FechaRegistro == fecha3);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de fecha no valido.");
                    }
                default:
                    throw new ArgumentOutOfRangeException($"La clase Cliente no contiene la propiedad {propiedad}.");
            }

        }
    }
}
