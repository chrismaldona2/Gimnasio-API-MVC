using Back.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Implementaciones.Contratos
{
    public interface IMembresiaService
    {
        Task<IEnumerable<Membresia>> ObtenerTodosLasMembresiasAsync();
        Task RegistrarMembresiaAsync(string descripcion, double duracionmeses, double precio);
        Task EliminarMembresiaAsync(int id);
        Task ModificarMembresiaAsync(Membresia membresia);

    }
}
