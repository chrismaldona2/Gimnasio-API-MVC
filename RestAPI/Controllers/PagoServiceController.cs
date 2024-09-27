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
    public class PagoServiceController : ControllerBase
    {
        private readonly IPagoService _pagoService;
        public PagoServiceController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> registrarPago(int IdCliente, int IdMembresia)
        {
            if (IdCliente == 0 || IdMembresia == 0)
            {
                return BadRequest("Id de membresía o cliente inválido.");
            }
            try
            {
                await _pagoService.RegistrarPagoAsync(IdCliente, IdMembresia);
                return Ok("Pago registrado exitosamente.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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


        [HttpGet("Lista")]
        public async Task<IActionResult> listaPagos()
        {
            try
            {
                var pagos = await _pagoService.ObtenerPagosAsync();
                if (!pagos.Any())
                {
                    return NotFound("No hay ningun pago registrado.");
                }
                return Ok(pagos);
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


        [HttpGet("PagosClientePorId")]
        public async Task<IActionResult> listaPagosPorIdCliente(int IdCliente)
        {
            try
            {
                var pagos = await _pagoService.ObtenerPagosPorIdClienteAsync(IdCliente);
                if (!pagos.Any())
                {
                    return NotFound("No hay ningun pago registrado.");
                }
                return Ok(pagos);
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


        [HttpGet("PagosClientePorDni")]
        public async Task<IActionResult> listaPagosPorDniCliente(string DniCliente)
        {
            try
            {
                var pagos = await _pagoService.ObtenerPagosPorDniClienteAsync(DniCliente);
                if (!pagos.Any())
                {
                    return NotFound("No hay ningun pago registrado.");
                }
                return Ok(pagos);
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
        public async Task<IActionResult> eliminarPago(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("El Id del pago es inválido.");
            }
            try
            {
                await _pagoService.EliminarPagoAsync(Id);
                return Ok("Pago eliminado exitosamente.");
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
                return BadRequest("Id de pago inválido.");
            }
            try
            {
                var pago = await _pagoService.BuscarPagoPorIdAsync(Id);
                if (pago != null)
                {
                    return Ok(pago);
                }

                return NotFound("El pago especificado no fue encontrado");

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
