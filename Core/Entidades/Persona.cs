using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public enum Sexo
    {
        Masculino = 0,
        Femenino = 1,
        Otro = 2
    }
    public class Persona
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public Sexo Sexo { get; set; }

    }
}
