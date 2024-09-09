﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entidades;

namespace Services.Contratos
{
    public interface IMembresiaService
    {
        Task<IEnumerable<Membresia>> ObtenerMembresiasAsync();
        Task RegistrarMembresiaAsync(string tipo, int duraciondias, double precio);
        Task EliminarMembresiaAsync(int id);
        Task ModificarMembresiaAsync(Membresia membresia);
    }
}