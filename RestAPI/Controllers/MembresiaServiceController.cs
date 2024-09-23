using Core.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models.DTOs;
using Services;
using Services.Contratos;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembresiaServiceController : ControllerBase
    {

        private readonly IMembresiaService _membresiaService;
        public MembresiaServiceController(IMembresiaService membresiaService)
        {
            _membresiaService = membresiaService;
        }
        //lista
        [HttpGet("Lista")]
        public async Task<IActionResult> listaMembresias()
        {
            try
            {
                var membresias = await _membresiaService.ObtenerMembresiasAsync();
                if (!membresias.Any())
                {
                    return NotFound("No hay ninguna membresía registrada.");
                }
                return Ok(membresias);
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

        //alta
        [HttpPost("Registrar")]
        public async Task<IActionResult> registrarMembresia([FromBody] MembresiaDTO membresiaDto)
        {
            if (membresiaDto == null)
            {
                return BadRequest("Los datos de la membresía son requeridos.");
            }

            try
            {
                await _membresiaService.RegistrarMembresiaAsync(membresiaDto.Tipo, membresiaDto.DuracionDias, membresiaDto.Precio);
                return Ok("Membresía registrada exitosamente.");
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
        public async Task<IActionResult> eliminarMembresia(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("El Id de membresía es inválido.");
            }
            try
            {
                await _membresiaService.EliminarMembresiaAsync(Id);
                return Ok("Membresía eliminada exitosamente.");
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
        public async Task<IActionResult> modificarMembresia(int Id, [FromBody] MembresiaDTO MembresiaDTO)
        {
            if (MembresiaDTO == null)
            {
                return BadRequest("Los datos de la membresía son requeridos.");
            }

            if (Id <= 0)
            {
                return BadRequest("El Id de membresía es inválido.");
            }

            var membresiaModificada = new Membresia()
            {
                Id = Id,
                Tipo = MembresiaDTO.Tipo,
                DuracionDias = MembresiaDTO.DuracionDias,
                Precio = MembresiaDTO.Precio
            };

            try
            {
                await _membresiaService.ModificarMembresiaAsync(membresiaModificada);
                return Ok("Membresía modificada exitosamente.");
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

        [HttpGet("BuscarID")]
        public async Task<IActionResult> buscarPorId(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("Id de membresía inválido.");
            }
            try
            {
                var membresia = await _membresiaService.BuscarMembresiaPorIdAsync(Id);
                if (membresia != null)
                {
                    return Ok(membresia);
                }

                return NotFound("La membresía especificada no fue encontrada");

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
