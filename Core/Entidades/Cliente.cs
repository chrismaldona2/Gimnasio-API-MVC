using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Cliente : Persona
    {
        public int Id { get; set; }
        public int? IdMembresia { get; set; }
        public DateTime? FechaVencimientoMembresia { get; set; }
        public Cliente() { }
        public Cliente(string dni, string nombre, string apellido, string email, string telefono, DateOnly fechanacimiento, Sexo sexo)
        {
            Dni = dni;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Telefono = telefono;
            FechaNacimiento = fechanacimiento;
            Sexo = sexo;
            IdMembresia = null;
            FechaVencimientoMembresia = null;
        }
    }
}
