using Back.Entidades;
using Back.Implementaciones.Contratos;
using GimnasioWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace GimnasioWebApp.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly AdminAPIService _adminAPIService;

        public AdministradorController(AdminAPIService adminAPIService)
        {
            _adminAPIService = adminAPIService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var nombreAdmin = TempData["NombreAdministrador"] as string;

            if (string.IsNullOrEmpty(nombreAdmin))
            {
                nombreAdmin = "buenas";
            }

            var model = new AdminDashboardViewModel
            {
                Nombre = nombreAdmin
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AutenticarAdmin(IniciarSesionAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model); 
            }

            try
            {
                var resultado = await _adminAPIService.AutenticarAdmin(model);
                if (resultado != null)
                {
                    TempData["NombreAdministrador"] = resultado;
                    TempData["SuccessMessage"] = "Inicio de sesión exitoso.";
                    return RedirectToAction("Dashboard"); 
                }
                else
                {
                    TempData["ErrorMessage"] = "Nombre de usuario o contraseña incorrectos.";
                    return View("Login", model); 
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al intentar autenticar al administrador: {ex.Message}";
                return View("Login", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarAdmin(RegistrarAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Registro", model); 
            }

            try
            {
                var resultado = await _adminAPIService.RegistrarAdmin(model);
                if (resultado == true)
                {
                    TempData["SuccessMessage"] = "Cuenta registrada con exitoso.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al registrar la cuenta, verifique los datos.";
                    return View("Registro", model);
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al registrar al administrador: {ex.Message}";
                return View("Registro", model);
            }

        }
    }
}
