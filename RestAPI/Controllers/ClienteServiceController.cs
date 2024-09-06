using Core.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
