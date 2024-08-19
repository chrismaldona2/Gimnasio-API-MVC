using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Entidades
{
    public class Administrador
    {
        [Key]
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Dni {  get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public Sexo Sexo { get; set; }
        public Administrador() { }
        public Administrador(string nombreusuario, string contraseña, string dni, string nombre, string apellido, DateOnly nacimiento, Sexo sexo)
        {
            NombreUsuario = nombreusuario;
            Contraseña = contraseña;
            Dni = dni;
            Nombre = nombre;
            Apellido = apellido;
            FechaNacimiento = nacimiento;
            Sexo = sexo;
        }
    }
}
