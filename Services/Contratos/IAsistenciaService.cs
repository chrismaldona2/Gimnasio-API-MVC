using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entidades;

namespace Services.Contratos
{
    public interface IAsistenciaService
    {
        Task<IEnumerable<Asistencia>> ObtenerAsistenciasAsync();
        Task<IEnumerable<Asistencia>> ObtenerAsistenciasIdClienteAsync(int id);
        Task<IEnumerable<Asistencia>> ObtenerAsistenciasDniClienteAsync(string dni);
        Task EliminarAsistenciaAsync(int id);
    }
}
