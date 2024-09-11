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
            if (string.IsNullOrWhiteSpace(usuario) || usuario.Contains(" "))
            {
                throw new ArgumentException("El nombre de usuario no puede contener espacios.");
            }

            if (string.IsNullOrWhiteSpace(contraseña) || contraseña.Contains(" "))
            {
                throw new ArgumentException("La contraseña no puede contener espacios.");
            }

            if (await _adminRepository.ObtenerAdminConUsuarioAsync(usuario) != null)
            {
                throw new UsuarioRegistradoException();
            }

            if (await _adminRepository.ObtenerAdminConDniAsync(dni) != null)
            {
                throw new DniRegistradoException();
            }

            var admin = new Administrador(usuario, BCrypt.Net.BCrypt.HashPassword(contraseña), dni, nombre, apellido, email, telefono, fechanacimiento, sexo);

            try
            {
                await _adminRepository.CrearAsync(admin);
                await _adminRepository.GuardarCambiosAsync();

            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error al registrar al administrador: {ex.Message}");
            }
        }

        //baja
        public async Task EliminarAdminAsync(int idAdminEliminar)
        {
            try
            {
                var adminExistente = await _adminRepository.EncontrarPorIDAsync(idAdminEliminar);
                if (adminExistente == null)
                {
                    throw new KeyNotFoundException("El administrador con el Id especificado no existe.");
                }
                await _adminRepository.EliminarAsync(idAdminEliminar);
                await _adminRepository.GuardarCambiosAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error al eliminar al administrador: {ex.Message}");
            }
        }

        //modificacion
        public async Task ModificarAdminAsync(Administrador adminModificar)
        {
            if (adminModificar == null)
            {
                throw new ArgumentNullException("El administrador a modificar no puede ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(adminModificar.Usuario) || adminModificar.Usuario.Contains(" "))
            {
                throw new ArgumentException("El nombre de usuario no puede contener espacios.");
            }

            if (string.IsNullOrWhiteSpace(adminModificar.Contraseña) || adminModificar.Contraseña.Contains(" "))
            {
                throw new ArgumentException("La contraseña no puede contener espacios.");
            }

            var usuarioExistente = await _adminRepository.ObtenerAdminConUsuarioAsync(adminModificar.Usuario);

            if (usuarioExistente != null && usuarioExistente.Usuario != adminModificar.Usuario)
            {
                throw new UsuarioRegistradoException();
            }

            var dniUsado = await _adminRepository.ObtenerAdminConDniAsync(adminModificar.Dni);
            if (dniUsado != null && dniUsado.Dni != adminModificar.Dni)
            {
                throw new DniRegistradoException();
            }

            try
            {
                var adminExistente = await _adminRepository.EncontrarPorIDAsync(adminModificar.Id);
                if (adminExistente == null)
                {
                    throw new KeyNotFoundException("El administrador con el Id especificado no existe.");
                }

                adminExistente.Usuario = adminModificar.Usuario;
                adminExistente.Contraseña = BCrypt.Net.BCrypt.HashPassword(adminModificar.Contraseña);
                adminExistente.Dni = adminModificar.Dni;
                adminExistente.Nombre = adminModificar.Nombre;
                adminExistente.Apellido = adminModificar.Apellido;
                adminExistente.Email = adminModificar.Email;
                adminExistente.Telefono = adminModificar.Telefono;
                adminExistente.FechaNacimiento = adminModificar.FechaNacimiento;
                adminExistente.Sexo = adminModificar.Sexo;

                await _adminRepository.ModificarAsync(adminExistente);
                await _adminRepository.GuardarCambiosAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error al modificar al administrador: {ex.Message}");
            }
        }

        //autenticacion
        public async Task<Administrador> AutenticarAdminAsync(string usuario, string contraseña)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contraseña))
            {
                throw new ArgumentException("El usuario y la contraseña no pueden estar vacíos.");
            }

            if (string.IsNullOrWhiteSpace(usuario) || usuario.Contains(" "))
            {
                throw new ArgumentException("El nombre de usuario no puede contener espacios.");
            }

            if (string.IsNullOrWhiteSpace(contraseña) || contraseña.Contains(" "))
            {
                throw new ArgumentException("La contraseña no puede contener espacios.");
            }

            try
            {
                var admin = await _adminRepository.ObtenerAdminConUsuarioAsync(usuario);

                if (admin == null || !BCrypt.Net.BCrypt.Verify(contraseña, admin.Contraseña))
                {
                    return null;
                }

                return admin;
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error al autenticar al administrador: {ex.Message}");
            }
        }

        //lista
        public async Task<IEnumerable<Administrador>> ObtenerAdministradoresAsync()
        {
            try
            {
                return await _adminRepository.ReturnListaAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }

        //buscar
        public async Task<Administrador> BuscarAdminPorUsuarioAsync(string usuario)
        {
            try
            {
                var admin = await _adminRepository.ObtenerAdminConUsuarioAsync(usuario);
                if (admin == null)
                {
                    throw new KeyNotFoundException("No hay un administrador registrado con ese DNI.");
                }
                return admin;
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }


        //buscar
        public async Task<Administrador> BuscarAdminPorDniAsync(string dni)
        {
            try
            {
                var admin = await _adminRepository.ObtenerAdminConUsuarioAsync(dni);

                if (admin == null)
                {
                    throw new KeyNotFoundException("No hay un administrador registrado con ese DNI.");
                }

                return admin;
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error inesperado al intentar realizar la acción: {ex.Message}");
            }
        }
    }
}