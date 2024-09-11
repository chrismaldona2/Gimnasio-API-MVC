using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Contratos;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciaServiceController : ControllerBase
    {
        private readonly IAsistenciaService _asistenciaService;
        public AsistenciaServiceController(IAsistenciaService asistenciaService)
        {
            _asistenciaService = asistenciaService;
        }

        //lista
        [HttpGet("Lista")]
        public async Task<IActionResult> listaAsistencias()
        {
            try
            {
                var asistencias = await _asistenciaService.ObtenerAsistenciasAsync();
                if (!asistencias.Any())
                {
                    return NotFound("No hay ninguna asistencia registrada.");
                }
                return Ok(asistencias);
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


        [HttpGet("AsistenciasClientePorId")]
        public async Task<IActionResult> listaAsistenciasPorIdCliente(int IdCliente)
        {
            try
            {
                var asistencias = await _asistenciaService.ObtenerAsistenciasIdClienteAsync(IdCliente);
                if (!asistencias.Any())
                {
                    return NotFound("El cliente no tiene ninguna asistencia registrada.");
                }
                return Ok(asistencias);
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


        [HttpGet("AsistenciasClientePorDni")]
        public async Task<IActionResult> listaAsistenciasPorDniCliente(string DniCliente)
        {
            try
            {
                var asistencias = await _asistenciaService.ObtenerAsistenciasDniClienteAsync(DniCliente);
                if (!asistencias.Any())
                {
                    return NotFound("El cliente no tiene ninguna asistencia registrada.");
                }
                return Ok(asistencias);
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

        [HttpDelete("Eliminar")]
        public async Task<IActionResult> eliminarAsistencia(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("El Id de asistencia es inválido.");
            }
            try
            {
                await _asistenciaService.EliminarAsistenciaAsync(Id);
                return Ok("Asistencia eliminada exitosamente.");
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

    }
}
