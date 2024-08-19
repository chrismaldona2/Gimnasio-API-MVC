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
    public class AdministradorService : IAdministradorService
    {
        private readonly IRepositorio<Administrador> _adminRepository;

        public AdministradorService(IRepositorio<Administrador> adminRepository)
        {
            _adminRepository = adminRepository;
        }

        //AUTENTICACIÓN
        public async Task<Administrador> AutenticarAdminAsync(string nombreusuario, string contraseña)
        {
            var adminQuery = _adminRepository.EncontrarPorCondicionAsync(a => a.NombreUsuario == nombreusuario);

            var admin = await adminQuery.FirstOrDefaultAsync();

            if (admin == null || !BCrypt.Net.BCrypt.Verify(contraseña, admin.Contraseña))
            {
                return null;
            }

            return admin;
        }

        //ALTA BAJA MODIFICACION
        public async Task RegistrarAdminAsync(string nombreusuario, string contraseña, string dni, string nombre, string apellido, DateOnly nacimiento, Sexo sexo)
        {
            if (_adminRepository.EncontrarPorCondicionAsync(a => a.NombreUsuario == nombreusuario).FirstOrDefault() != null)
            {
                throw new InvalidOperationException("El nombre de usuario ya está en uso");
            }

            if (string.IsNullOrEmpty(dni) || dni.Length != 8 || !dni.All(char.IsDigit))
            {
                throw new ArgumentException("DNI inválido. Debe ser un número entero de 8 dígitos");
            }

            if (_adminRepository.EncontrarPorCondicionAsync(a => a.Dni == dni).FirstOrDefault() != null)
            {
                throw new InvalidOperationException("Ya existe un administrador registrado con dicho DNI");
            }

            string contraseñacifrada = BCrypt.Net.BCrypt.HashPassword(contraseña);
            var adminNuevo = new Administrador(nombreusuario, contraseñacifrada, dni, nombre, apellido, nacimiento, sexo);
            await _adminRepository.CrearAsync(adminNuevo);
            await _adminRepository.GuardarCambiosAsync();
        }

        public async Task EliminarAdminAsync(int idAdminEliminar)
        {
            await _adminRepository.EliminarAsync(idAdminEliminar);
            await _adminRepository.GuardarCambiosAsync();
        }

        public async Task ModificarAdminAsync(Administrador adminModificar)
        {
            if (adminModificar != null)
            {
                if (_adminRepository.EncontrarPorCondicionAsync(a => a.NombreUsuario == adminModificar.NombreUsuario && a.Id != adminModificar.Id).FirstOrDefault() != null)
                {
                    throw new InvalidOperationException("El nombre de usuario ya está en uso por otro administrador");
                }

                if (string.IsNullOrEmpty(adminModificar.Dni) || adminModificar.Dni.Length != 8 || !adminModificar.Dni.All(char.IsDigit))
                {
                    throw new ArgumentException("DNI inválido. Debe ser un número entero de 8 dígitos");
                }

                if (_adminRepository.EncontrarPorCondicionAsync(a => a.Dni == adminModificar.Dni && a.Id != adminModificar.Id).FirstOrDefault() != null)
                {
                    throw new InvalidOperationException("El DNI especificado ya está en uso por otro administrador");
                }

                await _adminRepository.ModificarAsync(adminModificar);
                await _adminRepository.GuardarCambiosAsync();
            }
        }

    }
}