using API.Dto;
using API.Models;
using Back.Entidades;
using Back.Implementaciones;
using Back.Implementaciones.Contratos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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

        [HttpGet("ListadoClientes")]
        public async Task<IActionResult> ReturnClientes()
        {
            try
            {
                return Ok(await _clienteService.ObtenerTodosLosClientesAsync());

            }
            catch (Exception ex)
            {
                return BadRequest($"Error al intentar retornar el listado de clientes: {ex.Message}.");
            }
        }

        [HttpGet("BuscarPorID")]
        public async Task<IActionResult> BuscarClientePorID(int idCliente)
        {
            try
            {
                var cliente = await _clienteService.BuscarClientePorIDAsync(idCliente);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al buscar el cliente: {ex.Message}.");
            }
        }


        [HttpGet("BuscarPorDNI")]
        public async Task<IActionResult> BuscarClientePorDNI(string dniCliente)
        {
            try
            {
                var cliente = await _clienteService.BuscarClientesPorDniAsync(dniCliente);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al buscar el cliente: {ex.Message}.");
            }
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarCliente(ClienteDto clienteData)
        {
            try
            {
                await _clienteService.RegistrarClienteAsync(clienteData.Dni, clienteData.Nombre, clienteData.Apellido,
                    new DateOnly(clienteData.añoNacimiento, clienteData.mesNacimiento, clienteData.diaNacimiento),
                    (Sexo)clienteData.Sexo,
                    new DateOnly(clienteData.añoVencimiento, clienteData.mesVencimiento, clienteData.diaVencimiento));
                return Ok("Cliente registrado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el cliente: {ex.Message}.");
            }
        }

        [HttpDelete("Eliminar")]
        public async Task<IActionResult> EliminarCliente(int idCliente)
        {
            try
            {
                await _clienteService.EliminarClienteAsync(idCliente);
                return Ok("Cliente eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar al cliente: {ex.Message}.");
            }
        }

        [HttpPost("Modificar")]
        public async Task<IActionResult> ModificarCliente(ClienteModel clienteData)
        {
            try
            {
                var clienteModificado = new Cliente
                {
                    Id = clienteData.Id,
                    Dni = clienteData.Dni,
                    Nombre = clienteData.Nombre,
                    Apellido = clienteData.Apellido,
                    FechaNacimiento = new DateOnly(clienteData.añoNacimiento, clienteData.mesNacimiento, clienteData.diaNacimiento),
                    Sexo = (Sexo)clienteData.Sexo,
                    VencimientoMembresia = new DateOnly(clienteData.añoVencimiento, clienteData.mesVencimiento, clienteData.diaVencimiento)
                };
                await _clienteService.ModificarClienteAsync(clienteModificado);
                return Ok("Cliente modificado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al modificar al cliente: {ex.Message}.");
            }
        }
    }
}
