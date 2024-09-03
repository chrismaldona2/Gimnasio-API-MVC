using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Core.Entidades
{
    public class Administrador : Persona
    {
        public int Id {  get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public Administrador() { }
        public Administrador(string usuario, string contraseña, string dni, string nombre, string apellido, string email, string telefono, DateOnly fechanacimiento, Sexo sexo)
        {
            Usuario = usuario;
            Contraseña = contraseña;
            Dni = dni;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Telefono = telefono;
            FechaNacimiento = fechanacimiento;
            Sexo = sexo;
        }
    }
}
