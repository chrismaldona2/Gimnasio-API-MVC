using API.Dto;
using Back.Entidades;

namespace API.Models
{
    public class AdministradorModel
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int diaNacimiento { get; set; }
        public int mesNacimiento { get; set; }
        public int añoNacimiento { get; set; }
        public int Sexo { get; set; }
    }
}
