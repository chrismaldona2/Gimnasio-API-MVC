using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models.Administrador;
using WebApp.Models.Cliente;
using WebApp.Services.Contracts;

namespace WebApp.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IAdministradorAPIService _administradorApiService;
        private readonly IClienteAPIService _clienteService;

        public UsuarioController(IAdministradorAPIService administradorApiService, IClienteAPIService clienteService)
        {
            _administradorApiService = administradorApiService;
            _clienteService = clienteService;
        }


        public IActionResult LoginCliente()
        {
            HttpContext.Session.Clear();
            ClienteModel? model = null;
            return View(model);
        }


        public IActionResult LoginAdmin()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> BuscarCliente(string dniCliente)
        {
            var cliente = await _clienteService.BuscarClientePorDni(dniCliente);
            if (cliente != null)
            {
                return Json(cliente);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AutenticarAdmin(LoginAdminDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("LoginAdmin", model);
            }
            var respuesta = await _administradorApiService.AutenticarAdminAsync(model.Usuario, model.Contraseña);

            if (respuesta.Exitoso)
            {
                TempData["SuccessMessage"] = $"{respuesta.Mensaje}";

                var adminLoguado = await _administradorApiService.BuscarAdminPorUsuarioAsync(model.Usuario);

                HttpContext.Session.SetString("AdminLogueado", JsonConvert.SerializeObject(adminLoguado));

                return RedirectToAction("PanelInicio", "Administrador");
            }
            else
            {
                TempData["ErrorMessage"] = $"{respuesta.Mensaje}";
                return View("LoginAdmin", model);
            }
        }
    }
}
