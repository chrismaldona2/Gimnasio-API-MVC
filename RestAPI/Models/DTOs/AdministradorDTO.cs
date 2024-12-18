﻿namespace RestAPI.Models.DTOs
{

    public class AdministradorDTO
    {
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string FechaNacimiento { get; set; } // String format: "yyyy-MM-dd"
        public int Sexo { get; set; }
    }
}
