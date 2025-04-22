using Aplicacion.Consultas;
using Dominio.Interfaces;
using Dominio.Servicios;
using Infraestructura.FuentesDatos;
using Infraestructura.Persistencia.ContextoDB;
using Infraestructura.Persistencia.Repositorios;
using Infraestructura.Persistencia.UnidadDeTrabajo;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Servicios base
builder.Services.AddHttpClient();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ConsultaContarDatosExternos>());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar fuentes de datos
builder.Services.AddScoped<IFuenteDatosExternaVacantes, FuenteDatosMockarooVacantes>();
builder.Services.AddScoped<IFuenteDatosExternaCandidatos, FuenteDatosMockarooCandidatos>();

// Registrar servicios de dominio
builder.Services.AddScoped<IServicioEmparejamiento, ServicioEmparejamiento>();

// Configurar base de datos
builder.Services.AddDbContext<ContextoBdEmparejamiento>(options =>
    options.UseInMemoryDatabase("EmparejamientosDb")); // Para desarrollo inicial, luego cambiar a SQL Server

// Registrar repositorios y unidad de trabajo
builder.Services.AddScoped<IRepositorioEmparejamientos, RepositorioEmparejamientos>();
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

var app = builder.Build();

// Configure el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();