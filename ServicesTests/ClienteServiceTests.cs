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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Tests
{
    [TestClass()]
    public class ClienteServiceTests
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

        private IClienteRepositorio _clienteTestRepository;
        private ClienteService _clienteTestService;

        [TestInitialize]
        public void TestInitialize()
        {
            _testContext = getGimnasioInMemoryContext();
            _clienteTestRepository = new ClienteRepositorio(_testContext);
            _clienteTestService = new ClienteService(_clienteTestRepository);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _testContext.Database.EnsureDeleted();
            _testContext.Dispose();
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaRegistrarClienteCorrectamente()
        {
            var clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(0, clientes.Count());

            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);


            clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(1, clientes.Count());
            Assert.AreEqual("Christian", (await _clienteTestService.BuscarClientePorIdAsync(1)).Nombre);
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaRegistrarClienteCorrectamente2()
        {
            var clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(0, clientes.Count());

            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await _clienteTestService.RegistrarClienteAsync(
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechaNacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(2, clientes.Count());
            Assert.AreEqual("Christian", (await _clienteTestService.BuscarClientePorIdAsync(1)).Nombre);
            Assert.AreEqual("Mariano", (await _clienteTestService.BuscarClientePorIdAsync(2)).Nombre);
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaRegistrarClienteCorrectamente3()
        {
            var clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(0, clientes.Count());

            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await _clienteTestService.RegistrarClienteAsync(
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechaNacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(2, clientes.Count());
            Assert.AreEqual("Christian", (await _clienteTestService.BuscarClientePorIdAsync(1)).Nombre);
            Assert.AreEqual("Mariano", (await _clienteTestService.BuscarClientePorIdAsync(2)).Nombre);
        }


        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirFechasDeNacimientoSuperioresAHoy()
        {
            await Assert.ThrowsExceptionAsync<FechaNacimientoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@gmail.com",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: new DateOnly(2204, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirFechasDeNacimientoSuperioresAHoy2()
        {
            await Assert.ThrowsExceptionAsync<FechaNacimientoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@gmail.com",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: DateOnly.FromDateTime(DateTime.Now.AddDays(20)),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }



        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirColocarDNIUsado()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechaNacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            Assert.AreEqual(1, (await _clienteTestService.ObtenerClientesAsync()).Count());

            await Assert.ThrowsExceptionAsync<DniRegistradoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45490124",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@gmail.com",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });

            Assert.AreEqual(1, (await _clienteTestService.ObtenerClientesAsync()).Count());
        }


        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaQuitarEspaciosAlFinalOPrincipioDeStrings()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45490124    ",
                nombre: "Mariano     ",
                apellido: "    Perez",
                email: "     mariano10@gmail.com      ",
                telefono: "+54 3493 640312       ",
                fechaNacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            Assert.AreEqual(1, (await _clienteTestService.ObtenerClientesAsync()).Count());

            var clienteNuevo = await _clienteTestService.BuscarClientePorDniAsync("45490124");
            Assert.IsNotNull(clienteNuevo);
            Assert.AreEqual("Mariano", clienteNuevo.Nombre);
            Assert.AreEqual("Perez", clienteNuevo.Apellido);
            Assert.AreEqual("mariano10@gmail.com", clienteNuevo.Email);
            Assert.AreEqual("+54 3493 640312", clienteNuevo.Telefono);
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirNombresInvalidos()
        {
            var clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(0, clientes.Count());



            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45492726",
                    nombre: "Christian_",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@gmail.com",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });

            clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(0, clientes.Count());
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirNombresInvalidos2()
        {
            var clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(0, clientes.Count());

            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonad0",
                    email: "chris.ariel.maldonado@gmail.com",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });

            clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(0, clientes.Count());
        }


        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirNombresInvalidos3()
        {
            var clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(0, clientes.Count());

            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45492726",
                    nombre: "Christia.n",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@gmail.com",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });

            clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(0, clientes.Count());
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaPermitirNombresCompuestos()
        {
            var clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(0, clientes.Count());

            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian Ariel ",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);


            clientes = await _clienteTestService.ObtenerClientesAsync();
            Assert.AreEqual(1, clientes.Count());
            Assert.AreEqual("Christian Ariel", (await _clienteTestService.BuscarClientePorIdAsync(1)).Nombre);
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaPermitirNombresCompuestos2()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "40139872",
                nombre: "Lionel Andrés",
                apellido: "Messi Cuccitini",
                email: "liomessi10@gmail.com",
                telefono: "+54 3493 410583",
                fechaNacimiento: new DateOnly(1990, 4, 26),
                sexo: Core.Entidades.Sexo.Masculino);


            var messi = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(messi);
            Assert.AreEqual("Lionel Andrés", messi.Nombre);
            Assert.AreEqual("Messi Cuccitini", messi.Apellido);
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirEmailsInvalidos()
        {

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@gmailcom",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirEmailsInvalidos2()
        {

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel.  maldonado@gmail.com",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirEmailsInvalidos3()
        {

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@@gmail.com",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirEmailsInvalidos4()
        {

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "@gmail.com",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaImpedirEmailsInvalidos5()
        {

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                await _clienteTestService.RegistrarClienteAsync(
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@gmail.com..ar",
                    telefono: "+54 3493 439701",
                    fechaNacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarClienteAsync_DeberiaPermitirEmailsCompuestos()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com.ar",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);


            var chris = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(chris);
            Assert.AreEqual("chris.ariel.maldonado@gmail.com.ar", chris.Email);

        }


        [TestMethod]
        public async Task EliminarClienteAsync_DeberiaEliminarClienteCorrectamente()
        {
            Assert.AreEqual(0, (await _clienteTestService.ObtenerClientesAsync()).Count());

            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await _clienteTestService.RegistrarClienteAsync(
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechaNacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            var mariano = await _clienteTestService.BuscarClientePorDniAsync("45490124");

            Assert.IsNotNull(mariano);
            Assert.AreEqual(2, (await _clienteTestService.ObtenerClientesAsync()).Count());
            


            await _clienteTestService.EliminarClienteAsync(mariano.Id);

            Assert.AreEqual(1, (await _clienteTestService.ObtenerClientesAsync()).Count());
            Assert.IsNull(await _clienteTestService.BuscarClientePorDniAsync("45490124"));
        }

        [TestMethod]
        public async Task EliminarClienteAsync_DeberiaEliminarClienteCorrectamente2()
        {
            Assert.AreEqual(0, (await _clienteTestService.ObtenerClientesAsync()).Count());

            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            Assert.IsNotNull(await _clienteTestService.BuscarClientePorDniAsync("45492726"));
            Assert.AreEqual(1, (await _clienteTestService.ObtenerClientesAsync()).Count());

            await _clienteTestService.EliminarClienteAsync(1);

            Assert.AreEqual(0, (await _clienteTestService.ObtenerClientesAsync()).Count());
            Assert.IsNull(await _clienteTestService.BuscarClientePorDniAsync("45492726"));
        }

        [TestMethod]
        public async Task EliminarClienteAsync_DeberiaLanzarExcepcionSiClienteNoExiste()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            Assert.IsNotNull(await _clienteTestService.BuscarClientePorDniAsync("45492726"));
            Assert.AreEqual(1, (await _clienteTestService.ObtenerClientesAsync()).Count());


            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _clienteTestService.EliminarClienteAsync(2);
            });
        }

        [TestMethod]
        public async Task EliminarClienteAsync_DeberiaLanzarExcepcionSiClienteNoExiste2()
        {

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _clienteTestService.EliminarClienteAsync(150000);
            });
        }

        [TestMethod]
        public async Task EliminarClienteAsync_DeberiaLanzarExcepcionSiClienteNoExiste3()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            Assert.IsNotNull(await _clienteTestService.BuscarClientePorDniAsync("45492726"));
            Assert.AreEqual(1, (await _clienteTestService.ObtenerClientesAsync()).Count());

            await _clienteTestService.EliminarClienteAsync(1);
            Assert.IsNull(await _clienteTestService.BuscarClientePorDniAsync("45492726"));
            Assert.AreEqual(0, (await _clienteTestService.ObtenerClientesAsync()).Count());


            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _clienteTestService.EliminarClienteAsync(1);
            });
        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaModificarClienteCorrectamente()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Cristian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            Assert.AreEqual(1, (await _clienteTestService.ObtenerClientesAsync()).Count());

            var chris = await _clienteTestService.BuscarClientePorIdAsync(1);

            chris.Nombre = "Christian";
            await _clienteTestService.ModificarClienteAsync(chris);

            Assert.AreEqual("Christian", (await _clienteTestService.BuscarClientePorIdAsync(1)).Nombre);
        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaModificarClienteCorrectamente2()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await _clienteTestService.RegistrarClienteAsync(
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechaNacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            Assert.AreEqual(2, (await _clienteTestService.ObtenerClientesAsync()).Count());

            var chris = await _clienteTestService.BuscarClientePorIdAsync(1);
            var mariano = await _clienteTestService.BuscarClientePorIdAsync(2);

            chris.Email = "christian.maldonado.ices@gmail.com";
            mariano.Telefono = "+54 3493 409231";

            await _clienteTestService.ModificarClienteAsync(chris);
            await _clienteTestService.ModificarClienteAsync(mariano);
        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaQuitarEspaciosAlFinalOPrincipioDeStrings()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Cristian",
                apellido: "Mldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            var chris = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(chris);


            chris.Nombre = "Christian   ";
            chris.Apellido = "   Maldonado   ";
            chris.Telefono = "+54 3493 415282 ";

            await _clienteTestService.ModificarClienteAsync(chris);

            var adminActualizado = await _clienteTestService.BuscarClientePorIdAsync(1);

            Assert.AreEqual("Christian", adminActualizado.Nombre);
            Assert.AreEqual("Maldonado", adminActualizado.Apellido);
            Assert.AreEqual("+54 3493 415282", adminActualizado.Telefono);
        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaImpedirNombresInvalidos()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            var chris = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(chris);

            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                chris.Nombre = "Christi4n";
                await _clienteTestService.ModificarClienteAsync(chris);
            });
        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaImpedirNombresInvalidos2()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            var chris = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(chris);

            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                chris.Apellido = "Maldo.nado";
                await _clienteTestService.ModificarClienteAsync(chris);
            });
        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaPermitirNombresCompuestos()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "40139872",
                nombre: "Lionel Andrés",
                apellido: "Messi",
                email: "liomessi10@gmail.com",
                telefono: "+54 3493 410583",
                fechaNacimiento: new DateOnly(1990, 4, 26),
                sexo: Core.Entidades.Sexo.Masculino);


            var messi = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(messi);

            messi.Apellido = "Messi Cuccitini";
            await _clienteTestService.ModificarClienteAsync(messi);


            var clienteActualizado = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.AreEqual("Messi Cuccitini", clienteActualizado.Apellido);
        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaPermitirNombresCompuestos2()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "20034872",
                nombre: "José",
                apellido: "San Martin",
                email: "sanmartin2@gmail.com",
                telefono: "+54 3493 410375",
                fechaNacimiento: new DateOnly(1800, 6, 12),
                sexo: Core.Entidades.Sexo.Masculino);


            var sanmartin = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(sanmartin);

            sanmartin.Nombre = "José Francisco";
            sanmartin.Apellido = "San Martín y Matorras";
            await _clienteTestService.ModificarClienteAsync(sanmartin);


            var clienteActualizado = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.AreEqual("José Francisco", clienteActualizado.Nombre);
            Assert.AreEqual("San Martín y Matorras", clienteActualizado.Apellido);
        }



        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaImpedirEmailsInvalidos()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "20034872",
                nombre: "José Francisco",
                apellido: "San Martín y Matorras",
                email: "sanmartin2@gmail.com",
                telefono: "+54 3493 410375",
                fechaNacimiento: new DateOnly(1800, 6, 12),
                sexo: Core.Entidades.Sexo.Masculino);


            var sanmartin = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(sanmartin);

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                sanmartin.Email = "";
                await _clienteTestService.ModificarClienteAsync(sanmartin);
            });

        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaImpedirEmailsInvalidos2()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "20034872",
                nombre: "José Francisco",
                apellido: "San Martín y Matorras",
                email: "sanmartin2@gmail.com",
                telefono: "+54 3493 410375",
                fechaNacimiento: new DateOnly(1800, 6, 12),
                sexo: Core.Entidades.Sexo.Masculino);


            var sanmartin = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(sanmartin);

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                sanmartin.Email = "sanmartin2@@yahoo.com";
                await _clienteTestService.ModificarClienteAsync(sanmartin);
            });

        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaImpedirEmailsInvalidos3()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "20034872",
                nombre: "José Francisco",
                apellido: "San Martín y Matorras",
                email: "sanmartin2@gmail.com",
                telefono: "+54 3493 410375",
                fechaNacimiento: new DateOnly(1800, 6, 12),
                sexo: Core.Entidades.Sexo.Masculino);


            var sanmartin = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(sanmartin);

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                sanmartin.Email = "sanmartin2@yahoo..com";
                await _clienteTestService.ModificarClienteAsync(sanmartin);
            });

        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaImpedirEmailsInvalidos4()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "20034872",
                nombre: "José Francisco",
                apellido: "San Martín y Matorras",
                email: "sanmartin2@gmail.com",
                telefono: "+54 3493 410375",
                fechaNacimiento: new DateOnly(1800, 6, 12),
                sexo: Core.Entidades.Sexo.Masculino);


            var sanmartin = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(sanmartin);

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                sanmartin.Email = "@yahoo.com";
                await _clienteTestService.ModificarClienteAsync(sanmartin);
            });

        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaImpedirEmailsInvalidos5()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "20034872",
                nombre: "José Francisco",
                apellido: "San Martín y Matorras",
                email: "sanmartin2@gmail.com",
                telefono: "+54 3493 410375",
                fechaNacimiento: new DateOnly(1800, 6, 12),
                sexo: Core.Entidades.Sexo.Masculino);


            var sanmartin = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(sanmartin);

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                sanmartin.Email = "sanmartin2@gmail.com..ar";
                await _clienteTestService.ModificarClienteAsync(sanmartin);
            });

        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaPermitirEmailsCompuestos()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "20034872",
                nombre: "José Francisco",
                apellido: "San Martín y Matorras",
                email: "sanmartin2@gmail.com",
                telefono: "+54 3493 410375",
                fechaNacimiento: new DateOnly(1800, 6, 12),
                sexo: Core.Entidades.Sexo.Masculino);


            var sanmartin = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(sanmartin);

            sanmartin.Email = "sanmartin@gmail.com.ar";
            await _clienteTestService.ModificarClienteAsync(sanmartin);


            var sanmartinActualizado = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.AreEqual("sanmartin@gmail.com.ar", sanmartinActualizado.Email);
        }

        [TestMethod]
        public async Task ModificarClienteAsync_NecesitaDatosNoNulos()
        {

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _clienteTestService.ModificarClienteAsync(null);
            });
        }


        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaImpedirFechasDeNacimientoSuperioresAHoy()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "20034872",
                nombre: "José Francisco",
                apellido: "San Martín y Matorras",
                email: "sanmartin2@gmail.com",
                telefono: "+54 3493 410375",
                fechaNacimiento: new DateOnly(1800, 6, 12),
                sexo: Core.Entidades.Sexo.Masculino);


            var sanmartin = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(sanmartin);

            await Assert.ThrowsExceptionAsync<FechaNacimientoException>(async () =>
            {
                sanmartin.FechaNacimiento = new DateOnly(2100, 1, 20);
                await _clienteTestService.ModificarClienteAsync(sanmartin);
            });
        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaImpedirFechasDeNacimientoSuperioresAHoy2()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);


            var chris = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(chris);

            await Assert.ThrowsExceptionAsync<FechaNacimientoException>(async () =>
            {
                chris.FechaNacimiento = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
                await _clienteTestService.ModificarClienteAsync(chris);
            });
        }


        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaImpedirDniUsado()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await _clienteTestService.RegistrarClienteAsync(
                dni: "20034872",
                nombre: "José Francisco",
                apellido: "San Martín y Matorras",
                email: "sanmartin2@gmail.com",
                telefono: "+54 3493 410375",
                fechaNacimiento: new DateOnly(1800, 6, 12),
                sexo: Core.Entidades.Sexo.Masculino);


            var chris = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(chris);

            chris.Dni = "45492726";
            await _clienteTestService.ModificarClienteAsync(chris);

            await Assert.ThrowsExceptionAsync<DniRegistradoException>(async () =>
            {
                chris.Dni = "20034872";
                await _clienteTestService.ModificarClienteAsync(chris);
            });
        }

        [TestMethod]
        public async Task ModificarClienteAsync_DeberiaLanzarExcepcionSiClienteNoExiste()
        {
            await _clienteTestService.RegistrarClienteAsync(
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechaNacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            var chris = await _clienteTestService.BuscarClientePorIdAsync(1);
            Assert.IsNotNull(chris);

            chris.Id = 100000000;

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                chris.Nombre = "Christian Ariel";
                await _clienteTestService.ModificarClienteAsync(chris);
            });
        }
    }
}