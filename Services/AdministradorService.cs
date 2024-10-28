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
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradorRepositorio _adminRepository;

        public AdministradorService(IAdministradorRepositorio adminRepository)
        {
            _adminRepository = adminRepository;
        }

        //alta
        public async Task RegistrarAdminAsync(string usuario, string contraseña, string dni, string nombre, string apellido, string email, string telefono, DateOnly fechanacimiento, Sexo sexo)
        {
            if (string.IsNullOrWhiteSpace(usuario) || usuario.Trim().Contains(" "))
            {
                throw new ArgumentException("El nombre de usuario no puede contener espacios.");
            }

            if (string.IsNullOrWhiteSpace(contraseña) || contraseña.Trim().Contains(" "))
            {
                throw new ArgumentException("La contraseña no puede contener espacios.");
            }

            if (await _adminRepository.ObtenerAdminConUsuarioAsync(usuario.Trim()) != null)
            {
                throw new UsuarioRegistradoException();
            }

            if (await _adminRepository.ObtenerAdminConDniAsync(dni) != null)
            {
                throw new DniRegistradoException();
            }

            Regex ValidacionNombre = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(\s[A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$");

            if (!ValidacionNombre.IsMatch(nombre.Trim()) || !ValidacionNombre.IsMatch(apellido.Trim()))
            {
                throw new NombreInvalidoException("El nombre no debe contener números ni caracteres especiales.");
            }

            Regex ValidacionEmail = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!ValidacionEmail.IsMatch(email.Trim()) || email.Split('@')[1].Contains(".."))
            {
                throw new EmailInvalidoException();
            }

            if (fechanacimiento > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new FechaNacimientoException();
            }

            var admin = new Administrador(usuario.Trim(), BCrypt.Net.BCrypt.HashPassword(contraseña.Trim()), dni.Trim(), nombre.Trim(), apellido.Trim(), email.Trim(), telefono.Trim(), fechanacimiento, sexo, DateOnly.FromDateTime(DateTime.Now));


            await _adminRepository.CrearAsync(admin);
            await _adminRepository.GuardarCambiosAsync();

        }

        //baja
        public async Task EliminarAdminAsync(int idAdminEliminar)
        {

            var adminExistente = await _adminRepository.EncontrarPorIDAsync(idAdminEliminar);
            if (adminExistente == null)
            {
                throw new KeyNotFoundException("El administrador con el Id especificado no existe.");
            }
            await _adminRepository.EliminarAsync(idAdminEliminar);
            await _adminRepository.GuardarCambiosAsync();

        }

        //modificacion
        public async Task ModificarAdminAsync(Administrador adminModificar)
        {
            if (adminModificar == null)
            {
                throw new ArgumentNullException("El administrador a modificar no puede ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(adminModificar.Usuario) || adminModificar.Usuario.Trim().Contains(" "))
            {
                throw new ArgumentException("El nombre de usuario no puede contener espacios.");
            }

            if (string.IsNullOrWhiteSpace(adminModificar.Contraseña) || adminModificar.Contraseña.Trim().Contains(" "))
            {
                throw new ArgumentException("La contraseña no puede contener espacios.");
            }

            Regex ValidacionNombre = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(\s[A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$");

            if (!ValidacionNombre.IsMatch(adminModificar.Nombre.Trim()) || !ValidacionNombre.IsMatch(adminModificar.Apellido.Trim()))
            {
                throw new NombreInvalidoException("El nombre no debe contener números ni caracteres especiales.");
            }

            Regex ValidacionEmail = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!ValidacionEmail.IsMatch(adminModificar.Email.Trim()) || adminModificar.Email.Split('@')[1].Contains(".."))
            {
                throw new EmailInvalidoException();
            }

            if (adminModificar.FechaNacimiento > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new FechaNacimientoException();
            }


            var usuarioUsado = await _adminRepository.ObtenerAdminConUsuarioAsync(adminModificar.Usuario.Trim());
            if (usuarioUsado != null && usuarioUsado.Id != adminModificar.Id)
            {
                throw new UsuarioRegistradoException();
            }

            var dniUsado = await _adminRepository.ObtenerAdminConDniAsync(adminModificar.Dni);
            if (dniUsado != null && dniUsado.Id != adminModificar.Id)
            {
                throw new DniRegistradoException();
            }



            var adminExistente = await _adminRepository.EncontrarPorIDAsync(adminModificar.Id);
            if (adminExistente == null)
            {
                throw new KeyNotFoundException("El administrador con el Id especificado no existe.");
            }

            adminExistente.Usuario = adminModificar.Usuario.Trim();
            adminExistente.Contraseña = BCrypt.Net.BCrypt.HashPassword(adminModificar.Contraseña.Trim());
            adminExistente.Dni = adminModificar.Dni.Trim();
            adminExistente.Nombre = adminModificar.Nombre.Trim();
            adminExistente.Apellido = adminModificar.Apellido.Trim();
            adminExistente.Email = adminModificar.Email.Trim();
            adminExistente.Telefono = adminModificar.Telefono.Trim();
            adminExistente.FechaNacimiento = adminModificar.FechaNacimiento;
            adminExistente.Sexo = adminModificar.Sexo;

            await _adminRepository.ModificarAsync(adminExistente);
            await _adminRepository.GuardarCambiosAsync();

        }

        //autenticacion
        public async Task<Administrador> AutenticarAdminAsync(string usuario, string contraseña)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contraseña))
            {
                throw new ArgumentException("El usuario y la contraseña no pueden estar vacíos.");
            }

            if (string.IsNullOrWhiteSpace(usuario) || usuario.Trim().Contains(" "))
            {
                throw new ArgumentException("El nombre de usuario no puede contener espacios.");
            }

            if (string.IsNullOrWhiteSpace(contraseña) || contraseña.Trim().Contains(" "))
            {
                throw new ArgumentException("La contraseña no puede contener espacios.");
            }


            var admin = await _adminRepository.ObtenerAdminConUsuarioAsync(usuario.Trim());

            if (admin == null || !BCrypt.Net.BCrypt.Verify(contraseña.Trim(), admin.Contraseña))
            {
                return null;
            }

            return admin;

        }

        //lista
        public async Task<IEnumerable<Administrador>> ObtenerAdministradoresAsync() => await _adminRepository.ReturnListaAsync();

        //buscar
        public async Task<Administrador> BuscarAdminPorUsuarioAsync(string usuario) => await _adminRepository.ObtenerAdminConUsuarioAsync(usuario);


        public async Task<Administrador> BuscarAdminPorIdAsync(int id) => await _adminRepository.EncontrarPorIDAsync(id);


        public async Task<Administrador> BuscarAdminPorDniAsync(string dni) => await _adminRepository.ObtenerAdminConDniAsync(dni);


        public async Task<IEnumerable<Administrador>> FiltrarAdministradoresPorPropiedadAsync(string propiedad, string prefijo)
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
                                return await _adminRepository.EncontrarPorCondicionAsync(c => c.Id >= resultadoInt);
                            case "<=":
                                return await _adminRepository.EncontrarPorCondicionAsync(c => c.Id <= resultadoInt);
                            case ">":
                                return await _adminRepository.EncontrarPorCondicionAsync(c => c.Id > resultadoInt);
                            case "<":
                                return await _adminRepository.EncontrarPorCondicionAsync(c => c.Id < resultadoInt);
                            case "=":
                            default:
                                return await _adminRepository.EncontrarPorCondicionAsync(c => c.Id == resultadoInt);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de ID no valido.");
                    }

                case "usuario":
                    return await _adminRepository.EncontrarPorCondicionAsync(a => a.Usuario.StartsWith(trimedPrefijo));
                case "nombre":
                    return await _adminRepository.EncontrarPorCondicionAsync(a => a.Nombre.StartsWith(trimedPrefijo));
                case "apellido":
                    return await _adminRepository.EncontrarPorCondicionAsync(a => a.Apellido.StartsWith(trimedPrefijo));
                case "dni":
                    return await _adminRepository.EncontrarPorCondicionAsync(a => a.Dni.StartsWith(trimedPrefijo));
                case "email":
                    return await _adminRepository.EncontrarPorCondicionAsync(a => a.Email.StartsWith(trimedPrefijo));
                case "telefono":
                    return await _adminRepository.EncontrarPorCondicionAsync(a => a.Telefono.StartsWith(trimedPrefijo));
                case "sexo":
                    if (Enum.TryParse<Sexo>(trimedPrefijo, true, out var sexoValor))
                    {
                        return await _adminRepository.EncontrarPorCondicionAsync(a => a.Sexo == sexoValor);
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
                                return await _adminRepository.EncontrarPorCondicionAsync(a => a.FechaNacimiento >= fecha);
                            case "<=":
                                return await _adminRepository.EncontrarPorCondicionAsync(a => a.FechaNacimiento <= fecha);
                            case ">":
                                return await _adminRepository.EncontrarPorCondicionAsync(a => a.FechaNacimiento > fecha);
                            case "<":
                                return await _adminRepository.EncontrarPorCondicionAsync(a => a.FechaNacimiento < fecha);
                            case "=":
                            default:
                                return await _adminRepository.EncontrarPorCondicionAsync(a => a.FechaNacimiento == fecha);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de fecha no valido.");
                    }
                case "fecharegistro":
                    string valorFecha2 = trimedPrefijo.TrimStart('>', '<', '=', ' ');

                    if (DateOnly.TryParse(valorFecha2, out DateOnly fecha2))
                    {
                        switch (operador)
                        {
                            case ">=":
                                return await _adminRepository.EncontrarPorCondicionAsync(a => a.FechaRegistro >= fecha2);
                            case "<=":
                                return await _adminRepository.EncontrarPorCondicionAsync(a => a.FechaRegistro <= fecha2);
                            case ">":
                                return await _adminRepository.EncontrarPorCondicionAsync(a => a.FechaRegistro > fecha2);
                            case "<":
                                return await _adminRepository.EncontrarPorCondicionAsync(a => a.FechaRegistro < fecha2);
                            case "=":
                            default:
                                return await _adminRepository.EncontrarPorCondicionAsync(a => a.FechaRegistro == fecha2);
                        }
                    }
                    else
                    {
                        throw new FormatException("Formato de fecha no valido.");
                    }
                default:
                    throw new ArgumentOutOfRangeException($"La clase Administrador no contiene la propiedad {propiedad}.");
            }

        }
    }
}