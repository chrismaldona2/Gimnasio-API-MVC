using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Contratos;
using System.Linq;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticasGeneralesController : Controller
    {

        private readonly IPagoService _pagoService;
        private readonly IClienteService _clienteService;
        private readonly IAdministradorService _administradorService;
        public EstadisticasGeneralesController(IPagoService pagoService, IClienteService clienteService, IAdministradorService administradorService)
        {
            _pagoService = pagoService;
            _clienteService = clienteService;
            _administradorService = administradorService;
        }

        [HttpGet("GananciasDelUltimoMes")]
        public async Task<IActionResult> GananciasDelUltimoMes()
        {
            try
            {
                DateTime fechaInicio = DateTime.Now.AddDays(-30);

                var pagosFiltrados = await _pagoService.FiltrarPagosPorPropiedadAsync("fechapago", $">={fechaInicio}");

                double total = 0;
                if (pagosFiltrados.Any())
                {
                    total = pagosFiltrados.Sum(p => p.Monto);
                }
                return Ok(total);
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


        [HttpGet("ClientesNuevosUltimoMes")]
        public async Task<IActionResult> ClientesNuevosUltimoMes()
        {
            try
            {
                DateOnly fechaInicio = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));

                var clientesFiltrados = await _clienteService.FiltrarClientesPorPropiedadAsync("fecharegistro", $">={fechaInicio}");
                int total = clientesFiltrados.Count();

                return Ok(total);
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

        [HttpGet("AdministradoresNuevosUltimoMes")]
        public async Task<IActionResult> AdministradoresNuevosUltimoMes()
        {
            try
            {
                DateOnly fechaInicio = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));

                var adminsFiltrados = await _administradorService.FiltrarAdministradoresPorPropiedadAsync("fecharegistro", $">={fechaInicio}");
                int total = adminsFiltrados.Count();

                return Ok(total);
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
