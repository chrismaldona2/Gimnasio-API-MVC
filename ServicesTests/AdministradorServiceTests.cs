using Data.Contexts;
using Data.Repositorios.Contratos;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Services.Tests
{
    [TestClass]
    public class AdministradorServiceTests
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
        private IAdministradorRepositorio _adminTestRepository;
        private AdministradorService _adminTestService;

        [TestInitialize]
        public void TestInitialize()
        {
            _testContext = getGimnasioInMemoryContext();
            _adminTestRepository = new AdministradorRepositorio(_testContext);
            _adminTestService = new AdministradorService(_adminTestRepository);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _testContext.Database.EnsureDeleted();
            _testContext.Dispose();
        }


        // TESTS PARA REGISTRO DE ADMINS
        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaRegistrarAdminCorrectamente()
        {
            var listaAdmins = await _adminTestService.ObtenerAdministradoresAsync();
            Assert.AreEqual(0, listaAdmins.Count());


            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);


            var adminBuscado = await _adminTestService.BuscarAdminPorIdAsync(1);
            listaAdmins = await _adminTestService.ObtenerAdministradoresAsync();

            Assert.AreEqual(1, listaAdmins.Count());
            Assert.AreEqual("45492726", adminBuscado.Dni);
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaHashearContraseñaCorrectamente()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            var adminBuscado = await _adminTestService.BuscarAdminPorIdAsync(1);
            Assert.AreNotEqual("xd", adminBuscado.Contraseña);
        }


        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirCrearAdminConMismoDNI()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await Assert.ThrowsExceptionAsync<DniRegistradoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "jorgeperez2",
                    contraseña: "xd2",
                    dni: "45492726",
                    nombre: "Jorge",
                    apellido: "Perez",
                    email: "jorge.perez2@gmail.com",
                    telefono: "+54 3493 410375",
                    fechanacimiento: new DateOnly(2000, 4, 17),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }


        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirCrearAdminConMismoUsuario()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await Assert.ThrowsExceptionAsync<UsuarioRegistradoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "chrismaldona2",
                    contraseña: "xd2",
                    dni: "43492012",
                    nombre: "Jorge",
                    apellido: "Perez",
                    email: "jorge.perez2@gmail.com",
                    telefono: "+54 3493 410375",
                    fechanacimiento: new DateOnly(2000, 4, 17),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }


        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirUsuariosConEspaciosDentro()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "messi  10",
                    contraseña: "contraseña",
                    dni: "43090193",
                    nombre: "Lionel",
                    apellido: "Messi",
                    email: "messi10@gmail.com",
                    telefono: "+54 3493 410375",
                    fechanacimiento: new DateOnly(1995, 4, 17),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }


        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirUsuariosConEspaciosDentro2()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "   ",
                    contraseña: "contraseña",
                    dni: "43090193",
                    nombre: "Lionel",
                    apellido: "Messi",
                    email: "messi10@gmail.com",
                    telefono: "+54 3493 410375",
                    fechanacimiento: new DateOnly(1995, 4, 17),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }



        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirContraseñasConEspaciosDentro()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "messi10",
                    contraseña: "contra  seña",
                    dni: "43090193",
                    nombre: "Lionel",
                    apellido: "Messi",
                    email: "messi10@gmail.com",
                    telefono: "+54 3493 410375",
                    fechanacimiento: new DateOnly(1995, 4, 17),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirContraseñasConEspaciosDentro2()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "messi10",
                    contraseña: "",
                    dni: "43090193",
                    nombre: "Lionel",
                    apellido: "Messi",
                    email: "messi10@gmail.com",
                    telefono: "+54 3493 410375",
                    fechanacimiento: new DateOnly(1995, 4, 17),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaQuitarEspaciosAlFinalOFinalDeContraseña()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: " espacio  ",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var adminBuscado = await _adminTestService.BuscarAdminPorUsuarioAsync("messi10");
            Assert.IsNotNull(adminBuscado);
        }


        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaQuitarEspaciosAlFinalOPrincipioDeStrings()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "  anto10  ",
                contraseña: "espacio",
                dni: "    43090154   ",
                nombre: "Antonela",
                apellido: "Roccuzzo     ",
                email: "  anto10@gmail.com",
                telefono: "+54 3493 417575",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Femenino);

            var adminBuscado = await _adminTestService.BuscarAdminPorUsuarioAsync("anto10");
            Assert.IsNotNull(adminBuscado);
            Assert.AreEqual("43090154", adminBuscado.Dni);
            Assert.AreEqual("Roccuzzo", adminBuscado.Apellido);
            Assert.AreEqual("anto10@gmail.com", adminBuscado.Email);
        }


        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirNombresInvalidos()
        {
            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "anto10",
                    contraseña: "espacio",
                    dni: "43090154",
                    nombre: "Antonela454",
                    apellido: "Roccuzzo",
                    email: "anto10@gmail.com",
                    telefono: "+54 3493 417575",
                    fechanacimiento: new DateOnly(1995, 4, 17),
                    sexo: Core.Entidades.Sexo.Femenino);
            });
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirNombresInvalidos2()
        {
            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "anto10",
                    contraseña: "espacio",
                    dni: "43090154",
                    nombre: "Antonela",
                    apellido: "R0ccuzz0",
                    email: "anto10@gmail.com",
                    telefono: "+54 3493 417575",
                    fechanacimiento: new DateOnly(1995, 4, 17),
                    sexo: Core.Entidades.Sexo.Femenino);
            });
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirNombresInvalidos3()
        {
            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "anto10",
                    contraseña: "espacio",
                    dni: "43090154",
                    nombre: "@ntonel@",
                    apellido: "Roccuzzo",
                    email: "anto10@gmail.com",
                    telefono: "+54 3493 417575",
                    fechanacimiento: new DateOnly(1995, 4, 17),
                    sexo: Core.Entidades.Sexo.Femenino);
            });
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaPermitirNombresCompuestos()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel Andrés",
                apellido: "Messi Cuccitini",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var messi = await _adminTestService.BuscarAdminPorDniAsync("43090193");
            Assert.IsNotNull(messi);
            Assert.AreEqual("Lionel Andrés", messi.Nombre);
            Assert.AreEqual("Messi Cuccitini", messi.Apellido);
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaPermitirNombresCompuestos2()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "sanmartin20",
                contraseña: "contraseña",
                dni: "20034872",
                nombre: "José Francisco",
                apellido: "San Martín y Matorras",
                email: "sanmartin2@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1800, 6, 12),
                sexo: Core.Entidades.Sexo.Masculino);

            var sanmartin = await _adminTestService.BuscarAdminPorDniAsync("20034872");
            Assert.IsNotNull(sanmartin);
            Assert.AreEqual("José Francisco", sanmartin.Nombre);
            Assert.AreEqual("San Martín y Matorras", sanmartin.Apellido);
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirFechasDeNacimientoSuperioresAHoy()
        {
            await Assert.ThrowsExceptionAsync<FechaNacimientoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "thiagomessi10",
                    contraseña: "password",
                    dni: "46948312",
                    nombre: "Thiago",
                    apellido: "Messi",
                    email: "thiago@gmail.com",
                    telefono: "+54 3493 410375",
                    fechanacimiento: new DateOnly(2100, 4, 17),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirEmailsInvalidos()
        {

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "chrismaldona2",
                    contraseña: "xd2",
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@gmailcom",
                    telefono: "+54 3493 439701",
                    fechanacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirEmailsInvalidos2()
        {

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "chrismaldona2",
                    contraseña: "xd2",
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@@gmail.com",
                    telefono: "+54 3493 439701",
                    fechanacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirEmailsInvalidos3()
        {

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "chrismaldona2",
                    contraseña: "xd2",
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel  maldonado@gmail.com",
                    telefono: "+54 3493 439701",
                    fechanacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirEmailsInvalidos4()
        {

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "chrismaldona2",
                    contraseña: "xd2",
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "@gmail.com",
                    telefono: "+54 3493 439701",
                    fechanacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }

        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaImpedirEmailsInvalidos5()
        {

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                await _adminTestService.RegistrarAdminAsync(
                    usuario: "chrismaldona2",
                    contraseña: "xd2",
                    dni: "45492726",
                    nombre: "Christian",
                    apellido: "Maldonado",
                    email: "chris.ariel.maldonado@gmail..com",
                    telefono: "+54 3493 439701",
                    fechanacimiento: new DateOnly(2004, 1, 13),
                    sexo: Core.Entidades.Sexo.Masculino);
            });
        }


        [TestMethod]
        public async Task RegistrarAdminAsync_DeberiaPermitirEmailsCompuestos()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd2",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com.ar",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);


            var chris = await _adminTestService.BuscarAdminPorIdAsync(1);
            Assert.IsNotNull(chris);
            Assert.AreEqual("chris.ariel.maldonado@gmail.com.ar", chris.Email);

        }

        [TestMethod]
        public async Task EliminarAdminAsync_DeberiaEliminarAdminCorrectamente()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "thiagomessi10",
                contraseña: "password",
                dni: "46948312",
                nombre: "Thiago",
                apellido: "Messi",
                email: "thiago@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(2010, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var admin = await _adminTestService.BuscarAdminPorUsuarioAsync("thiagomessi10");

            await _adminTestService.EliminarAdminAsync(admin.Id);

            Assert.IsNull(await _adminTestService.BuscarAdminPorUsuarioAsync("thiagomessi10"));
        }


        [TestMethod]
        public async Task EliminarAdminAsync_DeberiaLanzarExcepcionSiAdminNoExiste()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _adminTestService.EliminarAdminAsync(10000000);
            });
        }



        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaModificarAdminCorrectamente()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var admin = await _adminTestService.BuscarAdminPorUsuarioAsync("messi10");

            Assert.AreEqual("messi10@gmail.com", admin.Email);
            admin.Email = "messiarg10@gmail.com";

            await _adminTestService.ModificarAdminAsync(admin);
            var busquedaNueva = await _adminTestService.BuscarAdminPorUsuarioAsync("messi10");

            Assert.AreEqual("messiarg10@gmail.com", busquedaNueva.Email);
        }


        [TestMethod]
        public async Task ModificarAdminAsync_NecesitaDatosNoNulos()
        {

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _adminTestService.ModificarAdminAsync(null);
            });
        }


        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirUsuariosConEspaciosDentro()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var admin = await _adminTestService.BuscarAdminPorUsuarioAsync("messi10");


            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                admin.Usuario = "usuario nuevo";
                await _adminTestService.ModificarAdminAsync(admin);
            });
        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirContraseñasConEspaciosDentro()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var admin = await _adminTestService.BuscarAdminPorUsuarioAsync("messi10");

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                admin.Contraseña = "con tra se ña";
                await _adminTestService.ModificarAdminAsync(admin);
            });
        }


        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirFechasDeNacimientoSuperioresAHoy()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var admin = await _adminTestService.BuscarAdminPorUsuarioAsync("messi10");

            await Assert.ThrowsExceptionAsync<FechaNacimientoException>(async () =>
            {
                admin.FechaNacimiento = new DateOnly(2400, 1, 1);
                await _adminTestService.ModificarAdminAsync(admin);
            });
        }



        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirColocarUsuarioUsado()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var admin = await _adminTestService.BuscarAdminPorUsuarioAsync("messi10");
            admin.Usuario = "chrismaldona2";
            await Assert.ThrowsExceptionAsync<UsuarioRegistradoException>(async () =>
            {
                await _adminTestService.ModificarAdminAsync(admin);
            });
        }



        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirColocarDNIUsado()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var messi = await _adminTestService.BuscarAdminPorUsuarioAsync("messi10");

            await Assert.ThrowsExceptionAsync<DniRegistradoException>(async () =>
            {
                messi.Dni = "45492726";
                await _adminTestService.ModificarAdminAsync(messi);
            });
        }



        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaLanzarExcepcionSiAdminNoExiste()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            var admin = await _adminTestService.BuscarAdminPorDniAsync("45492726");

            await _adminTestService.EliminarAdminAsync(admin.Id);

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                admin.Nombre = "Christian Ariel";
                await _adminTestService.ModificarAdminAsync(admin);
            });
        }


        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaHashearContraseñaCorrectamente()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            var admin = await _adminTestService.BuscarAdminPorDniAsync("45492726");
            admin.Contraseña = "contraseñasegura123";
            await _adminTestService.ModificarAdminAsync(admin);

            Assert.AreNotEqual("contraseñasegura123", (await _adminTestService.BuscarAdminPorDniAsync("45492726")).Contraseña);
        }


        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaQuitarEspaciosAlFinalOPrincipioDeStrings()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "  Christian ",
                apellido: "Maldonado ",
                email: "chris.ariel.maldonado@gmail.com        ",
                telefono: "     +54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            var admin = await _adminTestService.BuscarAdminPorDniAsync("45492726");
            admin.Usuario = "  chrismaldona2  ";
            await _adminTestService.ModificarAdminAsync(admin);

            var clienteActualizado = await _adminTestService.BuscarAdminPorDniAsync("45492726");
            Assert.AreEqual("chrismaldona2", clienteActualizado.Usuario);
            Assert.AreEqual("Christian", clienteActualizado.Nombre);
            Assert.AreEqual("Maldonado", clienteActualizado.Apellido);
            Assert.AreEqual("chris.ariel.maldonado@gmail.com", clienteActualizado.Email);
            Assert.AreEqual("+54 3493 439701", clienteActualizado.Telefono);
        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaQuitarEspaciosAlFinalOPrincipioDeContraseña()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            var admin = await _adminTestService.BuscarAdminPorDniAsync("45492726");
            admin.Contraseña = "  CONTRASEÑA56  ";
            await _adminTestService.ModificarAdminAsync(admin);


            //NO COMPARO DIRECTAMENTE PORQUE LA CONTRASEÑA ES AUTOMATICAMENTE HASHEADA AL MODIFICARSE
            Assert.IsNull(await _adminTestService.AutenticarAdminAsync("chrismaldona2", "xd"));
            Assert.IsNotNull(await _adminTestService.AutenticarAdminAsync("chrismaldona2", "CONTRASEÑA56"));
        }


        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirNombresInvalidos()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "anto10",
                contraseña: "espacio",
                dni: "43090154",
                nombre: "Antonela",
                apellido: "Roccuzzo",
                email: "anto10@gmail.com",
                telefono: "+54 3493 417575",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Femenino);

            var anto = await _adminTestService.BuscarAdminPorDniAsync("43090154");
            anto.Nombre = "@ntonela";

            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                await _adminTestService.ModificarAdminAsync(anto);
            });
        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirNombresInvalidos2()
        {

            await _adminTestService.RegistrarAdminAsync(
                usuario: "anto10",
                contraseña: "espacio",
                dni: "43090154",
                nombre: "Antonela",
                apellido: "Roccuzzo",
                email: "anto10@gmail.com",
                telefono: "+54 3493 417575",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Femenino);

            var anto = await _adminTestService.BuscarAdminPorDniAsync("43090154");
            anto.Nombre = "Antonela  ";
            anto.Apellido = "R0cuzz0";

            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                await _adminTestService.ModificarAdminAsync(anto);
            });

        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirNombresInvalidos3()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "anto10",
                contraseña: "espacio",
                dni: "43090154",
                nombre: "Antonela",
                apellido: "Roccuzzo",
                email: "anto10@gmail.com",
                telefono: "+54 3493 417575",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Femenino);

            var anto = await _adminTestService.BuscarAdminPorDniAsync("43090154");
            anto.Nombre = "Anton3l4  ";
            anto.Apellido = "2ocuzzo";

            await Assert.ThrowsExceptionAsync<NombreInvalidoException>(async () =>
            {
                await _adminTestService.ModificarAdminAsync(anto);
            });
        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaPermitirNombresCompuestos()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            var chris = await _adminTestService.BuscarAdminPorIdAsync(1);
            chris.Nombre = "Christian Ariel  ";

            await _adminTestService.ModificarAdminAsync(chris);

            Assert.AreEqual("Christian Ariel", (await _adminTestService.BuscarAdminPorIdAsync(1)).Nombre);

        }
        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaPermitirNombresCompuestos2()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var messi = await _adminTestService.BuscarAdminPorIdAsync(1);
            messi.Nombre = "Lionel Andrés   ";
            messi.Apellido = "Messi Cuccitini";

            await _adminTestService.ModificarAdminAsync(messi);

            Assert.AreEqual("Lionel Andrés", (await _adminTestService.BuscarAdminPorIdAsync(1)).Nombre);
            Assert.AreEqual("Messi Cuccitini", (await _adminTestService.BuscarAdminPorIdAsync(1)).Apellido);
        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirEmailsInvalidos()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "marianoPerez12",
                contraseña: "contra",
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechanacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            var mariano = await _adminTestService.BuscarAdminPorUsuarioAsync("marianoPerez12");

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                mariano.Email = "mariano10@@gmail.com";
                await _adminTestService.ModificarAdminAsync(mariano);
            });
        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirEmailsInvalidos2()
        {

            await _adminTestService.RegistrarAdminAsync(
                usuario: "marianoPerez12",
                contraseña: "contra",
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechanacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            var mariano = await _adminTestService.BuscarAdminPorUsuarioAsync("marianoPerez12");

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                mariano.Email = "@gmail.com";
                await _adminTestService.ModificarAdminAsync(mariano);
            });
        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirEmailsInvalidos3()
        {

            await _adminTestService.RegistrarAdminAsync(
                usuario: "marianoPerez12",
                contraseña: "contra",
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechanacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            var mariano = await _adminTestService.BuscarAdminPorUsuarioAsync("marianoPerez12");

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                mariano.Email = "mariano10@@gmailcom";
                await _adminTestService.ModificarAdminAsync(mariano);
            });
        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirEmailsInvalidos4()
        {

            await _adminTestService.RegistrarAdminAsync(
                usuario: "marianoPerez12",
                contraseña: "contra",
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechanacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            var mariano = await _adminTestService.BuscarAdminPorUsuarioAsync("marianoPerez12");

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                mariano.Email = "mariano10 @gmail.com";
                await _adminTestService.ModificarAdminAsync(mariano);
            });
        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaImpedirEmailsInvalidos5()
        {

            await _adminTestService.RegistrarAdminAsync(
                usuario: "marianoPerez12",
                contraseña: "contra",
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechanacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);

            var mariano = await _adminTestService.BuscarAdminPorUsuarioAsync("marianoPerez12");

            await Assert.ThrowsExceptionAsync<EmailInvalidoException>(async () =>
            {
                mariano.Email = "mariano10@gmail..com";
                await _adminTestService.ModificarAdminAsync(mariano);
            });
        }

        [TestMethod]
        public async Task ModificarAdminAsync_DeberiaPermitirEmailsCompuestos()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "marianoPerez12",
                contraseña: "contra",
                dni: "45490124",
                nombre: "Mariano",
                apellido: "Perez",
                email: "mariano10@gmail.com",
                telefono: "+54 3493 640312",
                fechanacimiento: new DateOnly(2003, 5, 23),
                sexo: Core.Entidades.Sexo.Masculino);


            var mariano = await _adminTestService.BuscarAdminPorIdAsync(1);
            Assert.IsNotNull(mariano);

            mariano.Email = "mariano10@gmail.com.mx";
            await _adminTestService.ModificarAdminAsync(mariano);


            var marianoActualizado = await _adminTestService.BuscarAdminPorIdAsync(1);
            Assert.AreEqual("mariano10@gmail.com.mx", marianoActualizado.Email);
        }


        [TestMethod]
        public async Task AutenticarAdminAsync_DeberiaAutenticarAdminCorrectamente()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            Assert.IsNotNull(await _adminTestService.AutenticarAdminAsync("chrismaldona2", "xd"));
        }


        [TestMethod]
        public async Task AutenticarAdminAsync_DeberiaAutenticarAdminCorrectamente2()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            Assert.IsNull(await _adminTestService.AutenticarAdminAsync("messi10", "password"));
            Assert.IsNull(await _adminTestService.AutenticarAdminAsync("messi105", "contraseña"));
        }


        [TestMethod]
        public async Task AutenticarAdminAsync_DeberiaImpedirDatosVacios()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _adminTestService.AutenticarAdminAsync("", null);
            });
        }


        [TestMethod]
        public async Task AutenticarAdminAsync_DeberiaImpedirUsuariosConEspaciosDentro()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _adminTestService.AutenticarAdminAsync("messi  10", "contraseña");
            });
        }

        [TestMethod]
        public async Task AutenticarAdminAsync_DeberiaImpedirUsuariosConEspaciosDentro2()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _adminTestService.AutenticarAdminAsync(" ", "contraseña");
            });
        }



        [TestMethod]
        public async Task AutenticarAdminAsync_DeberiaImpedirContraseñasConEspaciosDentro()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _adminTestService.AutenticarAdminAsync("messi10", "con tra se ña");
            });
        }


        [TestMethod]
        public async Task AutenticarAdminAsync_DeberiaImpedirContraseñasConEspaciosDentro2()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _adminTestService.AutenticarAdminAsync("messi10", "");
            });
        }



        [TestMethod]
        public async Task AutenticarAdminAsync_DeberiaQuitarEspaciosAlFinalOPrincipioDeDatos()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            Assert.IsNotNull(await _adminTestService.AutenticarAdminAsync("messi10", "     contraseña"));
            Assert.IsNotNull(await _adminTestService.AutenticarAdminAsync("   messi10   ", "contraseña "));
        }




        [TestMethod]
        public async Task ObtenerAdministradoresAsync_DeberiaDevolverListaDeAdminsRegistrados()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await _adminTestService.RegistrarAdminAsync(
                usuario: "thiagomessi10",
                contraseña: "password",
                dni: "46948312",
                nombre: "Thiago",
                apellido: "Messi",
                email: "thiago@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(2010, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var lista = (await _adminTestService.ObtenerAdministradoresAsync()).ToList();

            Assert.AreEqual("messi10", lista[0].Usuario);
            Assert.AreEqual("chrismaldona2", lista[1].Usuario);
            Assert.AreEqual(3, lista.Count());
        }



        [TestMethod]
        public async Task ObtenerAdministradoresAsync_DeberiaDevolverListaDeAdminsRegistrados2()
        {
            await _adminTestService.RegistrarAdminAsync(
                usuario: "messi10",
                contraseña: "contraseña",
                dni: "43090193",
                nombre: "Lionel",
                apellido: "Messi",
                email: "messi10@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(1995, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            await _adminTestService.RegistrarAdminAsync(
                usuario: "chrismaldona2",
                contraseña: "xd",
                dni: "45492726",
                nombre: "Christian",
                apellido: "Maldonado",
                email: "chris.ariel.maldonado@gmail.com",
                telefono: "+54 3493 439701",
                fechanacimiento: new DateOnly(2004, 1, 13),
                sexo: Core.Entidades.Sexo.Masculino);

            await _adminTestService.RegistrarAdminAsync(
                usuario: "thiagomessi10",
                contraseña: "password",
                dni: "46948312",
                nombre: "Thiago",
                apellido: "Messi",
                email: "thiago@gmail.com",
                telefono: "+54 3493 410375",
                fechanacimiento: new DateOnly(2010, 4, 17),
                sexo: Core.Entidades.Sexo.Masculino);

            var lista = (await _adminTestService.ObtenerAdministradoresAsync()).ToList();
            Assert.AreEqual(3, lista.Count());


            var thiago = await _adminTestService.BuscarAdminPorUsuarioAsync("thiagomessi10");
            await _adminTestService.EliminarAdminAsync(thiago.Id);


            lista = (await _adminTestService.ObtenerAdministradoresAsync()).ToList();
            Assert.AreEqual(2, lista.Count());

            Assert.IsNull(lista.FirstOrDefault(x => x.Usuario == "thiagomessi10"));
        }



        [TestMethod]
        public async Task BuscarAdminPorIdAsync_DeberiaRetornarNullSiAdminNoExiste()
        {
            var admin = await _adminTestService.BuscarAdminPorIdAsync(10000);
            Assert.IsNull(admin);
        }
    }
}