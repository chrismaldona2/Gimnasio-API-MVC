using Core.Entidades;
using RestAPI.Models.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;

namespace RestAPI.Models.Entidades
{
    public class ClienteModel
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public SexoModel Sexo { get; set; }
        public int IdMembresia { get; set; }
        public DateTime FechaVencimientoMembresia { get; set; }
    }
}