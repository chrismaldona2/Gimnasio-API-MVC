using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models.DTOs;
using Services.Contratos;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;
using Services;
using RestAPI.Models.Entidades;
using System.Net;

namespace RestAPI.Controllers
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
        //alta
        [HttpPost("Registrar")]
        public async Task<IActionResult> registrarAdministrador([FromBody] AdministradorDTO AdministradorDto)
        {
            if (AdministradorDto == null)
            {
                return BadRequest("Los datos del administrador son requeridos.");
            }

            try
            {
                await _administradorService.RegistrarAdminAsync(
                    AdministradorDto.Usuario,
                    AdministradorDto.Contraseña,
                    AdministradorDto.Dni,
                    AdministradorDto.Nombre,
                    AdministradorDto.Apellido,
                    AdministradorDto.Email,
                    AdministradorDto.Telefono,
                    DateOnly.ParseExact(AdministradorDto.FechaNacimiento, "yyyy-MM-dd"),
                    (Sexo)AdministradorDto.Sexo);
                return Ok("Administrador registrado exitosamente.");
            }
            catch (UsuarioRegistradoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DniRegistradoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (FechaNacimientoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inesperado del servidor: " + ex.Message);
            }
        }

        //baja
        [HttpDelete("Eliminar")]
        public async Task<IActionResult> eliminarAdmin(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("El Id del administrador es inválido.");
            }

            try
            {
                await _administradorService.EliminarAdminAsync(Id);
                return Ok("Administrador eliminado exitosamente.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inesperado del servidor: " + ex.Message);
            }
        }

        //modificacion
        [HttpPut("Modificar")]
        public async Task<IActionResult> modificarAdministrador(int Id, [FromBody] AdministradorDTO AdministradorDto)
        {
            if (AdministradorDto == null)
            {
                return BadRequest("Los datos del administrador son requeridos.");
            }

            if (Id <= 0)
            {
                return BadRequest("El Id del administrador es inválido.");
            }

            var adminModificado = new Administrador()
            {
                Id = Id,
                Usuario = AdministradorDto.Usuario,
                Contraseña = AdministradorDto.Contraseña,
                Dni = AdministradorDto.Dni,
                Nombre = AdministradorDto.Nombre,
                Apellido = AdministradorDto.Apellido,
                Email = AdministradorDto.Email,
                Telefono = AdministradorDto.Telefono,
                FechaNacimiento = DateOnly.ParseExact(AdministradorDto.FechaNacimiento, "yyyy-MM-dd"),
                Sexo = (Sexo)AdministradorDto.Sexo
            };

            try
            {

                await _administradorService.ModificarAdminAsync(adminModificado);
                return Ok("Administrador modificado exitosamente.");
            }
            catch (UsuarioRegistradoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DniRegistradoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (FechaNacimientoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inesperado del servidor: " + ex.Message);
            }
        }

        //lista
        [HttpGet("Lista")]
        public async Task<IActionResult> listaAdministradores()
        {
            try
            {
                var administradores = await _administradorService.ObtenerAdministradoresAsync();
                if (!administradores.Any())
                {
                    return NotFound("No hay ningun administrador registrado.");
                }

                //admin a dto para no mostrar contraseñas
                var administradoresModel = administradores.Select(admin => new AdministradorModel
                {
                    Id = admin.Id,
                    Usuario = admin.Usuario,
                    Dni = admin.Dni,
                    Nombre = admin.Nombre,
                    Apellido = admin.Apellido,
                    Email = admin.Email,
                    Telefono = admin.Telefono,
                    FechaNacimiento = admin.FechaNacimiento,
                    Sexo = (SexoModel)admin.Sexo
                }); ;
                return Ok(administradoresModel);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inesperado del servidor: " + ex.Message);
            }
        }


        //autenticacion
        [HttpGet("Autenticar")]
        public async Task<IActionResult> autenticarAdmin(string Usuario, string Contraseña)
        {
            try
            {
                var auth = await _administradorService.AutenticarAdminAsync(Usuario, Contraseña);

                if (auth != null)
                {
                    return Ok("Inicio de sesión exitoso.");
                }

                return NotFound("Nombre de usuario o contraseña incorrectos.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inesperado del servidor: " + ex.Message);
            }
        }

        //buscar por usuario
        [HttpGet("BuscarUsuario")]
        public async Task<IActionResult> buscarAdministradorUsuario(string Usuario)
        {
            try
            {
                var admin = await _administradorService.BuscarAdminPorUsuarioAsync(Usuario);
                var adminModel = new AdministradorModel
                {
                    Id = admin.Id,
                    Usuario = admin.Usuario,
                    Dni = admin.Dni,
                    Nombre = admin.Nombre,
                    Apellido = admin.Apellido,
                    Email = admin.Email,
                    Telefono = admin.Telefono,
                    FechaNacimiento = admin.FechaNacimiento,
                    Sexo = (SexoModel)admin.Sexo
                };
                return Ok(adminModel);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inesperado del servidor: " + ex.Message);
            }
        }

        //buscar por dni
        [HttpGet("BuscarDNI")]
        public async Task<IActionResult> buscarAdministradorDni(string Dni)
        {
            try
            {
                var admin = await _administradorService.BuscarAdminPorDniAsync(Dni);
                var adminModel = new AdministradorModel
                {
                    Id = admin.Id,
                    Usuario = admin.Usuario,
                    Dni = admin.Dni,
                    Nombre = admin.Nombre,
                    Apellido = admin.Apellido,
                    Email = admin.Email,
                    Telefono = admin.Telefono,
                    FechaNacimiento = admin.FechaNacimiento,
                    Sexo = (SexoModel)admin.Sexo
                };
                return Ok(adminModel);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inesperado del servidor: " + ex.Message);
            }
        }

        [HttpGet("BuscarID")]
        public async Task<IActionResult> buscarAdministradorId(int Id)
        {
            try
            {
                var admin = await _administradorService.BuscarAdminPorIdAsync(Id);
                var adminModel = new AdministradorModel
                {
                    Id = admin.Id,
                    Usuario = admin.Usuario,
                    Dni = admin.Dni,
                    Nombre = admin.Nombre,
                    Apellido = admin.Apellido,
                    Email = admin.Email,
                    Telefono = admin.Telefono,
                    FechaNacimiento = admin.FechaNacimiento,
                    Sexo = (SexoModel)admin.Sexo
                };
                return Ok(adminModel);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inesperado del servidor: " + ex.Message);
            }

        }
    }
}
