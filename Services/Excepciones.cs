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
}
