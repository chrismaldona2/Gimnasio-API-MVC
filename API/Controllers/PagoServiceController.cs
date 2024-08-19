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
    public class PagoServiceController : ControllerBase
    {
        private readonly IPagoService _pagoService;

        public PagoServiceController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarPago(PagoDto pagoData)
        {
            try
            {
                await _pagoService.RegistrarPagoAsync(pagoData.IdCliente, pagoData.IdMembresia, pagoData.ActualizarVencimientoCliente);
                return Ok("Pago registrado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el pago: {ex.Message}.");
            }
        }

        [HttpDelete("Eliminar")]
        public async Task<IActionResult> EliminarPago(int idPago)
        {
            try
            {
                await _pagoService.EliminarPagoAsync(idPago);
                return Ok("Pago eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar al pago: {ex.Message}.");
            }
        }

        [HttpGet("BuscarPorIDCliente")]
        public async Task<IActionResult> PagosPorIDCliente(int idCliente)
        {
            try
            {
                var pagos = await _pagoService.ObtenerPagosPorClienteAsync(idCliente);
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al buscar los pagos del cliente: {ex.Message}.");
            }
        }

    }
}
