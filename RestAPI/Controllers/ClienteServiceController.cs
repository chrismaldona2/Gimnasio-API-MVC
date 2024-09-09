using Core.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Models.DTOs;
using RestAPI.Models.Entidades;
using Services;
using Services.Contratos;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteServiceController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClienteServiceController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        //alta
        [HttpPost("Registrar")]
        public async Task<IActionResult> registrarCliente([FromBody] ClienteDTO clienteDto)
        {
            if (clienteDto == null)
            {
                return BadRequest("Los datos del administrador son requeridos.");
            }

            try
            {
                await _clienteService.RegistrarClienteAsync(
                    clienteDto.Dni,
                    clienteDto.Nombre,
                    clienteDto.Apellido,
                    clienteDto.Email,
                    clienteDto.Telefono,
                    new DateOnly(clienteDto.FechaNacimiento.Año, clienteDto.FechaNacimiento.Mes, clienteDto.FechaNacimiento.Dia),
                    (Sexo)clienteDto.Sexo);
                return Ok("Cliente registrado exitosamente.");
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
        public async Task<IActionResult> eliminarCliente(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El Id del cliente es inválido.");
            }
            try
            {
                await _clienteService.EliminarClienteAsync(id);
                return Ok("Cliente eliminado exitosamente.");
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
        public async Task<IActionResult> modificarCliente(int id, [FromBody] ClienteDTO clienteDto)
        {
            if (clienteDto == null)
            {
                return BadRequest("Los datos del administrador son requeridos.");
            }

            if (id <= 0)
            {
                return BadRequest("El Id del administrador es inválido.");
            }

            var clienteModificado = new Cliente()
            {
                Id = id,
                Dni = clienteDto.Dni,
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                Email = clienteDto.Email,
                Telefono = clienteDto.Telefono,
                FechaNacimiento = new DateOnly(clienteDto.FechaNacimiento.Año, clienteDto.FechaNacimiento.Mes, clienteDto.FechaNacimiento.Dia),
                Sexo = (Sexo)clienteDto.Sexo
            };

            try
            {
                await _clienteService.ModificarClienteAsync(clienteModificado);
                return Ok("Cliente modificado exitosamente.");
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
                var clientes = await _clienteService.ObtenerClientesAsync();
                return Ok(clientes);
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

        //buscar
        [HttpGet("BuscarPorDni")]
        public async Task<IActionResult> buscarPorDni(string dni)
        {
            if (dni.IsNullOrEmpty())
            {
                return BadRequest("El DNI del cliente es requerido.");
            }
            try
            {
                var cliente = await _clienteService.BuscarClientePorDniAsync(dni);
                if (cliente != null)
                {
                    return Ok(cliente);
                }

                return NotFound("Cliente no encontrado, revise los datos.");
                
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
