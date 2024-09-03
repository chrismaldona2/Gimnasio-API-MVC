using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios.Contratos
{
    public interface IRepositorio<T> where T : class
    {
        Task<IEnumerable<T>> ReturnListaAsync();
        Task<T> EncontrarPorIDAsync(int id);
        Task CrearAsync(T entidad);
        Task ModificarAsync(T entidad);
        Task EliminarAsync(int id);
        Task GuardarCambiosAsync();
        Task<IQueryable<T>> EncontrarPorCondicionAsync(Expression<Func<T, bool>> expression);

    }
}
