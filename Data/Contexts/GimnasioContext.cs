using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
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
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //cliente
            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Dni)
                .IsUnique();


            modelBuilder.Entity<Cliente>()
                .HasOne<Membresia>()
                .WithMany()
                .HasForeignKey(c => c.IdMembresia)
                .OnDelete(DeleteBehavior.Restrict);

            //admin
            modelBuilder.Entity<Administrador>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Administrador>()
                .HasIndex(a => a.Usuario)
                .IsUnique();

            modelBuilder.Entity<Administrador>()
                .HasIndex(a => a.Dni)
                .IsUnique();

            modelBuilder.Entity<Administrador>()
                .Property(a => a.Usuario)
                .IsRequired();

            modelBuilder.Entity<Administrador>()
                .Property(a => a.Contraseña)
                .IsRequired();

            //asistencia
            modelBuilder.Entity<Asistencia>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Asistencia>()
                .HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(a => a.IdCliente)
                .OnDelete(DeleteBehavior.Cascade);

            //membresia
            modelBuilder.Entity<Membresia>()
                .HasKey(m => m.Id);

            //pago
            modelBuilder.Entity<Pago>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Pago>()
                .HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pago>()
                .HasOne<Membresia>()
                .WithMany()
                .HasForeignKey(p => p.IdMembresia)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
