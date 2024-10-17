using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UsuarioRegistradoException : Exception
    {
        public UsuarioRegistradoException() : base("El nombre de usuario ya está registrado.") { }
    }

    public class DniRegistradoException : Exception
    {
        public DniRegistradoException() : base("El número de DNI ya está registrado.") { }
    }

    public class FechaNacimientoException : Exception 
    {
        public FechaNacimientoException() : base("Fecha de nacimiento inválida. Verifique los datos.") { }
    }

    public class NombreInvalidoException : Exception
    {
        public NombreInvalidoException() : base("Nombre invalido, verifique los datos e intente nuevamente.") { }
        public NombreInvalidoException(string message) : base(message) { }
    }

    public class EmailInvalidoException : Exception
    {
        public EmailInvalidoException() : base("Correo electrónico invalido, verifique los datos e intente nuevamente.") { }
    }
}
