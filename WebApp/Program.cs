using WebApp.Services;
using WebApp.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<APIService>();
builder.Services.AddHttpClient<IAdministradorAPIService, AdministradorAPIService>();
builder.Services.AddHttpClient<IClienteAPIService, ClienteAPIService>();
builder.Services.AddHttpClient<IMembresiaAPIService, MembresiaAPIService>();
builder.Services.AddHttpClient<IPagoAPIService, PagoAPIService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(35); 
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=LoginCliente}");

app.Run();
