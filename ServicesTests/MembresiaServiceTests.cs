using Data.Contexts;
using Data.Repositorios.Contratos;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tests
{
    [TestClass()]
    public class MembresiaServiceTests
    {
        private GimnasioContext getGimnasioInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<GimnasioContext>()
                .UseInMemoryDatabase(databaseName: "GimnasioTestDB")
                .Options;

            var context = new GimnasioContext(options);
            return context;

        }

        private GimnasioContext _testContext;
        private IMembresiaRepositorio _membresiaTestRepository;
        private MembresiaService _membresiaTestService;

        [TestInitialize]
        public void TestInitialize()
        {
            _testContext = getGimnasioInMemoryContext();
            _membresiaTestRepository = new MembresiaRepositorio(_testContext);
            _membresiaTestService = new MembresiaService(_membresiaTestRepository);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _testContext.Database.EnsureDeleted();
            _testContext.Dispose();
        }


        [TestMethod]
        public async Task RegistrarMembresiaAsync_DeberiaRegistrarNembresiaCorrectamente()
        {
            var listaMembresias = await _membresiaTestService.ObtenerMembresiasAsync();
            Assert.AreEqual(0, listaMembresias.Count());

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            var membresiaBuscada = await _membresiaTestService.BuscarMembresiaPorIdAsync(1);
            Assert.AreEqual("Mensual", membresiaBuscada.Tipo);
            Assert.AreEqual(30, membresiaBuscada.DuracionDias);
            Assert.AreEqual(15000, membresiaBuscada.Precio);

            listaMembresias = await _membresiaTestService.ObtenerMembresiasAsync();
            Assert.AreEqual(1, listaMembresias.Count());
        }


        [TestMethod]
        public async Task RegistrarMembresiaAsync_DeberiaRegistrarNembresiaCorrectamente2()
        {
            var listaMembresias = await _membresiaTestService.ObtenerMembresiasAsync();
            Assert.AreEqual(0, listaMembresias.Count());

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Anual",
                duraciondias: 365,
                precio: 100000);

            var membresiaBuscada1 = await _membresiaTestService.BuscarMembresiaPorIdAsync(1);
            var membresiaBuscada2 = await _membresiaTestService.BuscarMembresiaPorIdAsync(2);

            Assert.AreEqual("Mensual", membresiaBuscada1.Tipo);
            Assert.AreEqual("Anual", membresiaBuscada2.Tipo);

            listaMembresias = await _membresiaTestService.ObtenerMembresiasAsync();
            Assert.AreEqual(2, listaMembresias.Count());
        }


        [TestMethod]
        public async Task RegistrarMembresiaAsync_DeberiaImpedirPreciosMenoresACero()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: -3000);
            });
        }


        [TestMethod]
        public async Task RegistrarMembresiaAsync_DeberiaImpedirDuracionIgualOMenorACero()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 0,
                precio: 15000);
            });
        }

        [TestMethod]
        public async Task RegistrarMembresiaAsync_DeberiaImpedirDuracionIgualOMenorACero2()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: -60,
                precio: 15000);
            });
        }

        [TestMethod]
        public async Task RegistrarMembresiaAsync_DeberiaEliminarEspaciosAlInicioOFinalDelTipo()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual   ",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "     Anual    ",
                duraciondias: 365,
                precio: 100000);


            var membresia1 = await _membresiaTestService.BuscarMembresiaPorIdAsync(1);
            var membresia2 = await _membresiaTestService.BuscarMembresiaPorIdAsync(2);


            Assert.AreEqual("Mensual", membresia1.Tipo);
            Assert.AreEqual("Anual", membresia2.Tipo);
        }


        [TestMethod]
        public async Task RegistrarMembresiaAsync_DeberiaImpedirTipoNuloOVacio()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _membresiaTestService.RegistrarMembresiaAsync(
                    tipo: " ",
                    duraciondias: 30,
                    precio: 15000);
            });
        }

        [TestMethod]
        public async Task RegistrarMembresiaAsync_DeberiaImpedirTipoNuloOVacio2()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _membresiaTestService.RegistrarMembresiaAsync(
                    tipo: null,
                    duraciondias: 30,
                    precio: 15000);
            });
        }


        [TestMethod]
        public async Task EliminarMembresiaAsync_DeberiaEliminarMembresiaCorrectamente()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Anual",
                duraciondias: 365,
                precio: 100000);


            var listaMembresias = await _membresiaTestService.ObtenerMembresiasAsync();
            Assert.AreEqual(2, listaMembresias.Count());

            await _membresiaTestService.EliminarMembresiaAsync(1);
            listaMembresias = await _membresiaTestService.ObtenerMembresiasAsync();
            Assert.AreEqual(1, listaMembresias.Count());
        }


        [TestMethod]
        public async Task EliminarMembresiaAsync_DeberiaEliminarMembresiaCorrectamente2()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Anual",
                duraciondias: 365,
                precio: 100000);

            Assert.IsNotNull(await _membresiaTestService.BuscarMembresiaPorIdAsync(1));

            await _membresiaTestService.EliminarMembresiaAsync(1);

            Assert.IsNull(await _membresiaTestService.BuscarMembresiaPorIdAsync(1));
        }


        [TestMethod]
        public async Task EliminarMembresiaAsync_DeberiaLanzarExcepcionSiMembresiaNoExiste()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _membresiaTestService.EliminarMembresiaAsync(100000);
            });
        }

        [TestMethod]
        public async Task EliminarMembresiaAsync_DeberiaLanzarExcepcionSiMembresiaNoExiste2()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Anual",
                duraciondias: 365,
                precio: 100000);

            await _membresiaTestService.EliminarMembresiaAsync(1);

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _membresiaTestService.EliminarMembresiaAsync(1);
            });
        }



        [TestMethod]
        public async Task ModificarMembresiaAsync_DeberiaModificarNembresiaCorrectamente()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            var membresiaModificar = await _membresiaTestService.BuscarMembresiaPorIdAsync(1);
            Assert.AreEqual(15000, membresiaModificar.Precio);

            membresiaModificar.Precio = 20000;
            await _membresiaTestService.ModificarMembresiaAsync(membresiaModificar);

            Assert.AreNotEqual(15000, (await _membresiaTestService.BuscarMembresiaPorIdAsync(1)).Precio);
            Assert.AreEqual(20000, (await _membresiaTestService.BuscarMembresiaPorIdAsync(1)).Precio);
        }


        [TestMethod]
        public async Task ModificarMembresiaAsync_DeberiaModificarNembresiaCorrectamente2()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 90,
                precio: 50000);


            var membresia2 = await _membresiaTestService.BuscarMembresiaPorIdAsync(2);
            Assert.AreEqual("Mensual", membresia2.Tipo);

            membresia2.Tipo = "Trimestral";
            await _membresiaTestService.ModificarMembresiaAsync(membresia2);

            Assert.AreEqual("Mensual", (await _membresiaTestService.BuscarMembresiaPorIdAsync(1)).Tipo);
            Assert.AreEqual("Trimestral", (await _membresiaTestService.BuscarMembresiaPorIdAsync(2)).Tipo);
        }


        [TestMethod]
        public async Task ModificarMembresiaAsync_DeberiaModificarNembresiaCorrectamente3()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Trimestral",
                duraciondias: 90,
                precio: 40000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Anual",
                duraciondias: 360,
                precio: 100000);


            var membresia2 = await _membresiaTestService.BuscarMembresiaPorIdAsync(2);
            var membresia3 = await _membresiaTestService.BuscarMembresiaPorIdAsync(3);
            Assert.AreEqual(40000, membresia2.Precio);
            Assert.AreEqual(360, membresia3.DuracionDias);

            membresia2.Precio = 41500;
            membresia3.DuracionDias = 365;
            await _membresiaTestService.ModificarMembresiaAsync(membresia2);
            await _membresiaTestService.ModificarMembresiaAsync(membresia3);


            Assert.AreEqual(90, (await _membresiaTestService.BuscarMembresiaPorIdAsync(2)).DuracionDias);
            Assert.AreEqual(41500, (await _membresiaTestService.BuscarMembresiaPorIdAsync(2)).Precio);


            Assert.AreEqual(365, (await _membresiaTestService.BuscarMembresiaPorIdAsync(3)).DuracionDias);
            Assert.AreEqual(100000, (await _membresiaTestService.BuscarMembresiaPorIdAsync(3)).Precio);
        }




        [TestMethod]
        public async Task ModificarMembresiaAsync_DeberiaImpedirMembresiaEnNull()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Trimestral",
                duraciondias: 90,
                precio: 40000);

            var membresia2 = await _membresiaTestService.BuscarMembresiaPorIdAsync(2);

            membresia2 = null;

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _membresiaTestService.ModificarMembresiaAsync(membresia2);
            });
        }


        [TestMethod]
        public async Task ModificarMembresiaAsync_DeberiaImpedirTipoNuloOVacio()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Trimestral",
                duraciondias: 90,
                precio: 40000);

            var membresia2 = await _membresiaTestService.BuscarMembresiaPorIdAsync(2);

            membresia2.Tipo = null;

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _membresiaTestService.ModificarMembresiaAsync(membresia2);
            });
        }

        [TestMethod]
        public async Task ModificarMembresiaAsync_DeberiaImpedirTipoNuloOVacio2()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensual",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Trimestral",
                duraciondias: 90,
                precio: 40000);

            var membresia2 = await _membresiaTestService.BuscarMembresiaPorIdAsync(2);

            membresia2.Tipo = "  ";

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _membresiaTestService.ModificarMembresiaAsync(membresia2);
            });
        }


        [TestMethod]
        public async Task ModificarMembresiaAsync_DeberiaImpedirPrecioMenorACero()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _membresiaTestService.RegistrarMembresiaAsync(
                    tipo: "Mensual",
                    duraciondias: 30,
                    precio: -10000);
            });
        }


        [TestMethod]
        public async Task ModificarMembresiaAsync_DeberiaImpedirDuracionMenorOIgualACero()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _membresiaTestService.RegistrarMembresiaAsync(
                    tipo: "Mensual",
                    duraciondias: 0,
                    precio: 12000);
            });
        }

        [TestMethod]
        public async Task ModificarMembresiaAsync_DeberiaImpedirDuracionMenorOIgualACero2()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _membresiaTestService.RegistrarMembresiaAsync(
                    tipo: "Mensual",
                    duraciondias: -30,
                    precio: 12000);
            });
        }


        [TestMethod]
        public async Task ModificarMembresiaAsync_DeberiaEliminarEspaciosAlInicioOFinalDelTipo()
        {
            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Mensuall",
                duraciondias: 30,
                precio: 15000);

            await _membresiaTestService.RegistrarMembresiaAsync(
                tipo: "Aaanual",
                duraciondias: 365,
                precio: 100000);

            var membresia1 = await _membresiaTestService.BuscarMembresiaPorIdAsync(1);
            var membresia2 = await _membresiaTestService.BuscarMembresiaPorIdAsync(2);


            membresia1.Tipo = "Mensual       ";
            membresia2.Tipo = "        Anual    ";
            await _membresiaTestService.ModificarMembresiaAsync(membresia1);
            await _membresiaTestService.ModificarMembresiaAsync(membresia2);


            var membresiaActualizada1 = await _membresiaTestService.BuscarMembresiaPorIdAsync(1);
            var membresiaActualizada2 = await _membresiaTestService.BuscarMembresiaPorIdAsync(2);
            Assert.AreEqual("Mensual", membresiaActualizada1.Tipo);
            Assert.AreEqual("Anual", membresiaActualizada2.Tipo);
        }
    }
}