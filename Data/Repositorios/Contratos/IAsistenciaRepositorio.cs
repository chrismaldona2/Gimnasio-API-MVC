using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios.Contratos
{
    public interface IAsistenciaRepositorio : IRepositorio<Asistencia>
    {
        Task<IEnumerable<Asistencia>> ObtenerAsistenciaPorClienteAsync(int idCliente);
    }
}
