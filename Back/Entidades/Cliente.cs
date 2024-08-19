using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Entidades
{
    public enum Sexo
    {
        Masculino,
        Femenino,
        Otro
    }
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public Sexo Sexo { get; set; }
        public DateOnly VencimientoMembresia { get; set; }
        public bool Vencido => DateOnly.FromDateTime(DateTime.Now) > VencimientoMembresia;

        public Cliente() { }
        public Cliente(string dni, string nombre, string apellido, DateOnly nacimiento, Sexo sexo, DateOnly vencimientoMembresia)
        {
            Dni = dni;
            Nombre = nombre;
            Apellido = apellido;
            FechaNacimiento = nacimiento;
            Sexo = sexo;
            VencimientoMembresia = vencimientoMembresia;
        }
    }
}
