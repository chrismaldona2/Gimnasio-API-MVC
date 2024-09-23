﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models.Administrador;
using WebApp.Models.Cliente;
using WebApp.Models.Membresia;
using WebApp.Models.Pago;
using WebApp.Services.Contracts;
using WebApp.ViewPermissions;

namespace WebApp.Controllers
{
    [ValidarSesionAdmin]
    public class AdministradorController : Controller
    {
        private readonly IAdministradorAPIService _administradorService;
        private readonly IClienteAPIService _clienteService;
        private readonly IMembresiaAPIService _membresiaService;
        private readonly IPagoAPIService _pagoService;
        public AdministradorController(IAdministradorAPIService administradorService, IClienteAPIService clienteService, IMembresiaAPIService membresiaService, IPagoAPIService pagoService)
        {
            _administradorService = administradorService;
            _clienteService = clienteService;
            _membresiaService = membresiaService;
            _pagoService = pagoService;
        }


        // PANTALLAS
        ////de inicio
        public IActionResult PanelInicio()
        {
            var adminLogueado = HttpContext.Session.GetString("AdminLogueado");

            var nombreAdminLogueado = adminLogueado != null ? JsonConvert.DeserializeObject<AdminModel>(adminLogueado).Nombre : null;

            string nombreAdmin = nombreAdminLogueado ?? "Administrador";

            return View("PanelInicio", (nombreAdmin.Trim()).Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
        }

        ////de ayuda
        public IActionResult PanelAyuda()
        {
            return View();
        }

        ////de perfil
        public IActionResult PanelPerfil()
        {
            return View();
        }

        ////de administrador
        public async Task<IActionResult> PanelAdministradores()
        {
            var listaAdministradores = await _administradorService.ListaAdministradores();

            PanelAdminModel model = new()
            {
                ListaAdministradores = listaAdministradores ?? new()
            };

            return View("PanelAdministradores", model);
        }

        ////de membresias
        public async Task<IActionResult> PanelMembresias()
        {
            var listaMembresias = await _membresiaService.ListaMembresias();

            PanelMembresiasModel model = new()
            {
                ListaMembresias = listaMembresias ?? new()
            };

            return View("PanelMembresias", model);
        }
        ////de clientes
        public async Task<IActionResult> PanelClientes()
        {
            var listaClientes = await _clienteService.ListaClientes();

            PanelClientesModel model = new()
            {
                ListaClientes = listaClientes ?? new()
            };
            return View("PanelClientes", model);
        }        
        ////de pagos
        public async Task<IActionResult> PanelPagos()
        {
            var listaPagos = await _pagoService.ListaPagos();

            PanelPagosModel model = new()
            {
                ListaPagos = listaPagos ?? new()
            };
            return View("PanelPagos", model);
        }








        //METODOS COMUNES ENTRE PANTALLAS

        [HttpGet]
        public async Task<IActionResult> BuscarCliente(int idCliente)
        {
            var cliente = await _clienteService.BuscarClientePorId(idCliente);
            if (cliente != null)
            {
                return Json(cliente);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> BuscarMembresia(int idMembresia)
        {
            var membresia = await _membresiaService.BuscarMembresiaPorId(idMembresia);
            if (membresia != null)
            {
                return Json(membresia);
            }
            return NotFound();
        }






        //METODOS DE LA PANTALLA DE ADMINISTRADOR
        [HttpPost]
        public async Task<IActionResult> RegistrarAdmin(PanelAdminModel model)
        {
            if (ModelState.IsValid)
            {

                AdminModel data = new()
                {
                    Usuario = model.Usuario,
                    Contraseña = model.Contraseña,
                    Dni = model.Dni,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    FechaNacimiento = model.FechaNacimiento,
                    Sexo = model.Sexo
                };
                var respuesta = await _administradorService.RegistrarAdminAsync(data);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelAdministradores");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarAdmin(PanelAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var respuesta = await _administradorService.EliminarAdminAsync(model.Id);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelAdministradores");
        }

        [HttpPost]
        public async Task<IActionResult> ModificarAdmin(PanelAdminModel model)
        {
            if (ModelState.IsValid)
            {
                AdminModel data = new()
                {
                    Id = model.Id,
                    Usuario = model.Usuario,
                    Contraseña = model.Contraseña,
                    Dni = model.Dni,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    FechaNacimiento = model.FechaNacimiento,
                    Sexo = model.Sexo
                };
                var respuesta = await _administradorService.ModificarAdminAsync(data);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelAdministradores");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelAdministradores");
        }

        [HttpGet]
        public async Task<IActionResult> BuscarAdmin(int idAdmin)
        {
            var admin = await _administradorService.BuscarAdminPorIdAsync(idAdmin);
            if (admin != null)
            {
                return Json(admin);
            }
            return NotFound();
        }






        //METODOS DE LA PANTALLA DE MEMBRESIAS
        [HttpPost]
        public async Task<IActionResult> RegistrarMembresia(PanelMembresiasModel model)
        {
            if (ModelState.IsValid)
            {

                MembresiaModel data = new()
                {
                    Tipo = model.Tipo,
                    DuracionDias = model.DuracionDias,
                    Precio = model.Precio,
                };
                var respuesta = await _membresiaService.RegistrarMembresiaAsync(data);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelMembresias");
        }

        [HttpPost]
        public async Task<IActionResult> ModificarMembresia(PanelMembresiasModel model)
        {
            if (ModelState.IsValid)
            {
                MembresiaModel data = new()
                {
                    Id = model.Id,
                    Tipo = model.Tipo,
                    DuracionDias = model.DuracionDias,
                    Precio = model.Precio,
                };
                var respuesta = await _membresiaService.ModificarMembresiaAsync(data);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelMembresias");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarMembresia(PanelMembresiasModel model)
        {
            if (ModelState.IsValid)
            {
                var respuesta = await _membresiaService.EliminarMembresiaAsync(model.Id);

                if (respuesta.Exitoso)
                {
                    TempData["SuccessMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
                else
                {
                    TempData["ErrorMessage"] = respuesta.Mensaje;
                    return RedirectToAction("PanelMembresias");
                }
            }
            TempData["ErrorMessage"] = "Error inesperado.";
            return RedirectToAction("PanelMembresias");
        }




    }
}