using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Back.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Back
{
    public class GimnasioContext : DbContext
    {
        public GimnasioContext() { }
        public GimnasioContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=GimnasioBD;Integrated Security=True;TrustServerCertificate=true;");
            }
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<Pago> Pagos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pago>()
                .HasOne<Cliente>() 
                .WithMany() 
                .HasForeignKey(p => p.IdCliente) 
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Pago>()
                .HasOne<Membresia>() 
                .WithMany() 
                .HasForeignKey(p => p.IdMembresia) 
                .OnDelete(DeleteBehavior.Restrict); 
        }

    }
}
