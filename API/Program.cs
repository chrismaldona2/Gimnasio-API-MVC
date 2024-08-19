using Back;
using Back.Entidades;
using Back.Repositorios;
using Back.Repositorios.Contratos;
using Back.Implementaciones;
using Back.Implementaciones.Contratos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

// repositorios
builder.Services.AddScoped<IRepositorio<Cliente>, ClienteRepositorio>();
builder.Services.AddScoped<IRepositorio<Pago>, PagoRepositorio>();
builder.Services.AddScoped<IRepositorio<Membresia>, MembresiaRepositorio>();
builder.Services.AddScoped<IRepositorio<Administrador>, AdministradorRepositorio>();

// servicios
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<IMembresiaService, MembresiaService>();
builder.Services.AddScoped<IAdministradorService, AdministradorService>();

// db
builder.Services.AddDbContext<GimnasioContext>(options => options.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=GimnasioBD;Integrated Security=True;TrustServerCertificate=true"), ServiceLifetime.Scoped);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
