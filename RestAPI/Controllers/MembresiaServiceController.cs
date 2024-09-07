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

        //lista
        [HttpGet("Lista")]
        public async Task<IActionResult> listaMembresias()
        {
            try
            {
                var membresias = await _membresiaService.ObtenerMembresiasAsync();
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
    }
}
