using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositorios.Contratos;
using Data.Contexts;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios
{
    public class MembresiaRepositorio : Repositorio<Membresia>, IRepositorio<Membresia>
    {
        public MembresiaRepositorio(GimnasioContext context) : base(context) { }
    }
}
