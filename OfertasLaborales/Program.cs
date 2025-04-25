// --- USINGS NECESARIOS ---
using Aplicacion.Consultas;         // Para la clase usada en AddMediatR
using Aplicacion.Interfaces;        // Para IEnviadorNotificaciones
using Dominio.Interfaces;
using Dominio.Servicios;
using Infraestructura.FuentesDatos;
using Infraestructura.Notificaciones;   // Para EnviadorNotificacionesConsola
using Infraestructura.Persistencia;
using Infraestructura.Persistencia.Repositorios;
using MediatR;
using Microsoft.EntityFrameworkCore;

// --- Configuración del Builder ---
var builder = WebApplication.CreateBuilder(args);

// --- Registro de Servicios (Inyección de Dependencias) ---

// Caché en Memoria (Lo tenías antes, aseguramos que esté)
builder.Services.AddMemoryCache();

// 1. HttpClientFactory (Para Fuentes de Datos Externas)
builder.Services.AddHttpClient();

// 2. MediatR (Para patrón CQRS)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ConsultaContarDatosExternos>()); // O tu clase de referencia en Aplicacion

// 3. DbContext (Usando Base de Datos en Memoria)
builder.Services.AddDbContext<ContextoBdEmparejamiento>(options =>
    options.UseInMemoryDatabase("BaseDatosEmparejamientos"));

// 4. Unidad de Trabajo y Repositorios propios del Dominio
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
builder.Services.AddScoped<IRepositorioEmparejamientos, RepositorioEmparejamientos>();
// --- Añadido para Notificaciones ---
builder.Services.AddScoped<IRepositorioNotificacionesEmparejamiento, RepositorioNotificacionesEmparejamiento>();
// ----------------------------------

// 5. Fuentes de Datos Externas (Implementaciones de Infraestructura)
builder.Services.AddScoped<IFuenteDatosExternaVacantes, FuenteDatosMockarooVacantes>();
builder.Services.AddScoped<IFuenteDatosExternaCandidatos, FuenteDatosMockarooCandidatos>();

// 6. Servicios de Dominio (Implementaciones)
builder.Services.AddScoped<IServicioEmparejamiento, ServicioEmparejamiento>();
// --- Añadido para Notificaciones ---
builder.Services.AddScoped<IServicioSeleccionNotificaciones, ServicioSeleccionNotificaciones>();
// ----------------------------------

// 7. Servicios de Infraestructura (Implementaciones de Interfaces de Aplicación)
// --- Añadido para Notificaciones ---
builder.Services.AddScoped<IEnviadorNotificaciones, EnviadorNotificacionesConsola>(); // Usando la implementación de Consola
// ----------------------------------


// 8. Servicios para API Controllers y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Construcción de la Aplicación ---
var app = builder.Build();

// --- Configuración del Pipeline de Peticiones HTTP ---

// Habilitar Swagger solo en Desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- Middlewares Estándar ---

// Comentado según tu preferencia actual para desarrollo local sin HTTPS configurado
// app.UseHttpsRedirection();

// app.UseAuthorization(); // Descomenta si añades autenticación/autorización

// Mapear las peticiones a los Controllers
app.MapControllers();

// --- Ejecutar la Aplicación ---
app.Run();