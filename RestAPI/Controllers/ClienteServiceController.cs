﻿using Core.Entidades;
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
                    DateOnly.ParseExact(clienteDto.FechaNacimiento, "yyyy-MM-dd"),
                    (Sexo)clienteDto.Sexo);
                return Ok("Cliente registrado exitosamente.");
            }
            catch (FechaNacimientoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DniRegistradoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (NombreInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EmailInvalidoException ex)
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
        public async Task<IActionResult> eliminarCliente(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("El Id del cliente es inválido.");
            }
            try
            {
                await _clienteService.EliminarClienteAsync(Id);
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
        public async Task<IActionResult> modificarCliente(int Id, [FromBody] ClienteDTO ClienteDto)
        {
            if (ClienteDto == null)
            {
                return BadRequest("Los datos del administrador son requeridos.");
            }

            if (Id <= 0)
            {
                return BadRequest("El Id del administrador es inválido.");
            }

            var clienteModificado = new Cliente()
            {
                Id = Id,
                Dni = ClienteDto.Dni,
                Nombre = ClienteDto.Nombre,
                Apellido = ClienteDto.Apellido,
                Email = ClienteDto.Email,
                Telefono = ClienteDto.Telefono,
                FechaNacimiento = DateOnly.ParseExact(ClienteDto.FechaNacimiento, "yyyy-MM-dd"),
                Sexo = (Sexo)ClienteDto.Sexo
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
            catch (FechaNacimientoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NombreInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EmailInvalidoException ex)
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
        public async Task<IActionResult> listaClientes()
        {
            try
            {
                var clientes = await _clienteService.ObtenerClientesAsync();
                if (!clientes.Any())
                {
                    return NotFound("No hay ningun cliente registrado.");
                }
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

        //lista
        [HttpGet("ListaConMembresiaVencida")]
        public async Task<IActionResult> listaClientesMembresiaVencida()
        {
            try
            {
                var clientes = await _clienteService.ObtenerClientesConMembresiaVencidaAsync();
                if (!clientes.Any())
                {
                    return NotFound("No hay clientes con su membresía vencida.");
                }

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
        [HttpGet("BuscarDNI")]
        public async Task<IActionResult> buscarPorDni(string Dni)
        {
            if (Dni.IsNullOrEmpty())
            {
                return BadRequest("El DNI del cliente es requerido.");
            }
            try
            {
                var cliente = await _clienteService.BuscarClientePorDniAsync(Dni);
                if (cliente != null)
                {
                    return Ok(cliente);
                }
                
                return NotFound("El cliente especificado no fue encontrado");

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
        public async Task<IActionResult> buscarPorId(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("Id de cliente inválido.");
            }
            try
            {
                var cliente = await _clienteService.BuscarClientePorIdAsync(Id);
                if (cliente != null)
                {
                    return Ok(cliente);
                }

                return NotFound("El cliente especificado no fue encontrado");

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

        [HttpGet("FiltrarClientesPorPropiedad")]
        public async Task<IActionResult> filtrarClientesPorPropiedad(string propiedad, string prefijo)
        {
            try
            {
                var clientesFiltrados = await _clienteService.FiltrarClientesPorPropiedadAsync(propiedad, prefijo);

                if (!clientesFiltrados.Any())
                {
                    return NotFound($"No se encontraron clientes cuya propiedad '{propiedad}' verifique '{prefijo}'.");
                }

                return Ok(clientesFiltrados);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
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
