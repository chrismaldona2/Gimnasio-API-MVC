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
    public class MembresiaController : ControllerBase
    {
        private readonly IMembresiaService _membresiaService;

        public MembresiaController(IMembresiaService membresiaService)
        {
            _membresiaService = membresiaService;
        }

        [HttpGet("ListadoMembresias")]
        public async Task<IActionResult> ReturnMembresias()
        {
            try
            {
                return Ok(await _membresiaService.ObtenerTodosLasMembresiasAsync());

            }
            catch (Exception ex)
            {
                return BadRequest($"Error al intentar retornar el listado de membresias: {ex.Message}.");
            }
        }
        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarMembresia(MembresiaDto membresiaData)
        {
            try
            {
                await _membresiaService.RegistrarMembresiaAsync(membresiaData.Descripcion, membresiaData.DuracionEnMeses, membresiaData.Precio);
                return Ok("Membresía registrada exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar la membresía: {ex.Message}.");
            }
        }

        [HttpDelete("Eliminar")]
        public async Task<IActionResult> EliminarMembresia(int idMembresia)
        {
            try
            {
                await _membresiaService.EliminarMembresiaAsync(idMembresia);
                return Ok("Membresía eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar la membresía: {ex.Message}.");
            }
        }

        [HttpPost("Modificar")]
        public async Task<IActionResult> ModificarMembresia(MembresiaModel membresiaData)
        {
            try
            {
                var membresiaModificada = new Membresia
                {
                    Id = membresiaData.Id,
                    Descripcion = membresiaData.Descripcion,
                    DuracionEnMeses = membresiaData.DuracionEnMeses,
                    Precio = membresiaData.Precio
                };
                await _membresiaService.ModificarMembresiaAsync(membresiaModificada);
                return Ok("Cliente modificado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al modificar al cliente: {ex.Message}.");
            }
        }
    }
}
