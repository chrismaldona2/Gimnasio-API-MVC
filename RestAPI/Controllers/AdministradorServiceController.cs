using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models.DTOs;
using Services.Contratos;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;
using Services;
using RestAPI.Models.Entidades;

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
        public async Task<IActionResult> registrarAdministrador(AdministradorDTO administradorDto)
        {
            if (administradorDto == null)
            {
                return BadRequest("Los datos del administrador son requeridos.");
            }

            try
            {
                await _administradorService.RegistrarAdminAsync(
                    administradorDto.Usuario,
                    administradorDto.Contraseña,
                    administradorDto.Dni,
                    administradorDto.Nombre,
                    administradorDto.Apellido,
                    administradorDto.Email,
                    administradorDto.Telefono,
                    new DateOnly(administradorDto.FechaNacimiento.Año, administradorDto.FechaNacimiento.Mes, administradorDto.FechaNacimiento.Dia),
                    (Sexo)administradorDto.Sexo);
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
        public async Task<IActionResult> EliminarAdmin(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El Id del administrador es inválido.");
            }

            try
            {
                await _administradorService.EliminarAdminAsync(id);
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
        public async Task<IActionResult> ModificarAdministrador(int id, AdministradorDTO administradorDto)
        {
            if (administradorDto == null)
            {
                return BadRequest("Los datos del administrador son requeridos.");
            }

            if (id <= 0)
            {
                return BadRequest("El Id del administrador es inválido.");
            }

            var adminModificado = new Administrador()
            {
                Id = id,
                Usuario = administradorDto.Usuario,
                Contraseña = administradorDto.Contraseña,
                Dni = administradorDto.Dni,
                Nombre = administradorDto.Nombre,
                Apellido = administradorDto.Apellido,
                Email = administradorDto.Email,
                Telefono = administradorDto.Telefono,
                FechaNacimiento = new DateOnly(administradorDto.FechaNacimiento.Año, administradorDto.FechaNacimiento.Mes, administradorDto.FechaNacimiento.Dia),
                Sexo = (Sexo)administradorDto.Sexo
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

        //lista
        [HttpGet("Buscar")]
        public async Task<IActionResult> buscarAdministrador(string Usuario)
        {
            try
            {
                var admin = await _administradorService.BuscarAdminPorUsuarioAsync(Usuario);
                
                if (admin != null)
                {
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
                return NotFound("Usuario no encontrado.");
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
