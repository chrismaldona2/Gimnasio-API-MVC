using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Repositorios.Contratos;
using Data.Contexts;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        protected GimnasioContext _context { get; set; }
        public Repositorio(GimnasioContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<T>> ReturnListaAsync()
        {
            return await this._context.Set<T>().ToListAsync();
        }

        public async Task<T> EncontrarPorIDAsync(int id)
        {
            return await this._context.Set<T>().FindAsync(id);
        }

        public async Task CrearAsync(T entidad)
        {
            await this._context.Set<T>().AddAsync(entidad);
        }
        public async Task ModificarAsync(T entidad)
        {
            this._context.Set<T>().Update(entidad);
        }
        public async Task EliminarAsync(int id)
        {
            var entidad = await EncontrarPorIDAsync(id);
            if (entidad != null)
            {
                this._context.Set<T>().Remove(entidad);
            }
        }
        public async Task GuardarCambiosAsync()
        {
            try
            {
                await this._context.SaveChangesAsync();
                return;
            }
            catch
            {
                throw new Exception("Error al guardar los cambios.");
            }
        }
        public async Task<IQueryable<T>> EncontrarPorCondicionAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.FromResult(this._context.Set<T>().Where(expression));
        }
    }
}
