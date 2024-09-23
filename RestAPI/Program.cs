using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Services.Contratos;
using Services;
using Core.Entidades;
using Data.Repositorios.Contratos;
using Data.Repositorios;

var builder = WebApplication.CreateBuilder(args);

//repositorios
builder.Services.AddDbContext<GimnasioContext>(options => options.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=GimnasioBD;Integrated Security=True;TrustServerCertificate=true"), ServiceLifetime.Scoped);
builder.Services.AddScoped<IAdministradorRepositorio, AdministradorRepositorio>();
builder.Services.AddScoped<IAsistenciaRepositorio, AsistenciaRepositorio>();
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IMembresiaRepositorio, MembresiaRepositorio>();
builder.Services.AddScoped<IPagoRepositorio, PagoRepositorio>();

//servicios
builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IAsistenciaService, AsistenciaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IMembresiaService, MembresiaService>();
builder.Services.AddScoped<IPagoService, PagoService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

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
