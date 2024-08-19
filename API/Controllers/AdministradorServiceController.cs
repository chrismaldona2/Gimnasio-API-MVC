using Back.Entidades;
using Back.Implementaciones.Contratos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Dto;
using API.Models;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradorServiceController : ControllerBase
    {
        private readonly IAdministradorService _administradorService;

        public AdministradorServiceController(IAdministradorService administradorService)
        {
            _administradorService = administradorService;
        }

        [HttpPost("Autenticar")]
        public async Task<IActionResult> AutenticarAdmin(AutenticacionDto adminData)
        { 
            try
            {
                var resultado = await _administradorService.AutenticarAdminAsync(adminData.NombreUsuario, adminData.Contraseña);
                if (resultado == null)
                {
                    return Unauthorized();
                }
                return Ok(resultado);
            } catch (Exception ex)
            {
                return BadRequest($"Error al intentar autenticar al administrador: {ex.Message}.");
            }
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarAdmin(AdministradorDto adminData)
        {
            try
            {
                await _administradorService.RegistrarAdminAsync(adminData.NombreUsuario, adminData.Contraseña, adminData.Dni, adminData.Nombre, adminData.Apellido, new DateOnly(adminData.añoNacimiento, adminData.mesNacimiento, adminData.diaNacimiento), (Sexo)adminData.Sexo);
                return Ok("Administrador registrado exitosamente.");
            } catch (Exception ex)
            {
                return BadRequest($"Error al registrar al administrador: {ex.Message}.");
            }
        }

        [HttpDelete("Eliminar")]
        public async Task<IActionResult> EliminarAdmin(int idAdmin)
        {
            try
            {
                await _administradorService.EliminarAdminAsync(idAdmin);
                return Ok("Administrador eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar al administrador: {ex.Message}.");
            }
        }

        [HttpPost("Modificar")]
        public async Task<IActionResult> ModificarAdmin(AdministradorModel adminData)
        {
            try
            {   
                var adminModificado = new Administrador
                {
                    Id = adminData.Id,
                    NombreUsuario = adminData.NombreUsuario,
                    Contraseña = BCrypt.Net.BCrypt.HashPassword(adminData.Contraseña),
                    Dni = adminData.Dni,
                    Nombre = adminData.Nombre,
                    Apellido = adminData.Apellido,
                    FechaNacimiento = new DateOnly(adminData.añoNacimiento, adminData.mesNacimiento, adminData.diaNacimiento),
                    Sexo = (Sexo)adminData.Sexo
                };
                await _administradorService.ModificarAdminAsync(adminModificado);
                return Ok("Administrador modificado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al modificar al administrador: {ex.Message}.");
            }
        }
    }
}
