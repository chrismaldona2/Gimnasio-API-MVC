using API.Dto;
using Back.Entidades;

namespace API.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int diaNacimiento { get; set; }
        public int mesNacimiento { get; set; }
        public int añoNacimiento { get; set; }
        public int Sexo { get; set; }
        public int diaVencimiento { get; set; }
        public int mesVencimiento { get; set; }
        public int añoVencimiento { get; set; }
    }
}
