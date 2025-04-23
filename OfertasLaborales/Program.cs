// --- USINGS NECESARIOS ---
using Aplicacion.Consultas; // Para la clase usada en AddMediatR
using Dominio.Interfaces;
using Dominio.Servicios;     // Para ServicioEmparejamiento
using Infraestructura.FuentesDatos;
using Infraestructura.Persistencia; // Para ContextoBdEmparejamiento, UnidadDeTrabajo
using Infraestructura.Persistencia.Repositorios; // Para RepositorioEmparejamientos
using MediatR;
using Microsoft.EntityFrameworkCore; // Para UseInMemoryDatabase

// --- Configuración del Builder ---
var builder = WebApplication.CreateBuilder(args);

// --- Registro de Servicios (Inyección de Dependencias) ---
builder.Services.AddMemoryCache(); // Para caché en memoria
// 1. HttpClientFactory (Para Fuentes de Datos Externas)
builder.Services.AddHttpClient();

// 2. MediatR (Para patrón CQRS)
// Asegúrate que ConsultaContarDatosExternos o una clase similar exista en Aplicacion
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ConsultaContarDatosExternos>());

// 3. DbContext (Usando Base de Datos en Memoria por ahora)
// Perfecto para desarrollo y pruebas iniciales sin configurar una BD real.
builder.Services.AddDbContext<ContextoBdEmparejamiento>(options =>
    options.UseInMemoryDatabase("BaseDatosEmparejamientos")); // Nombre de la BD en memoria

// 4. Unidad de Trabajo y Repositorios propios del Dominio
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
builder.Services.AddScoped<IRepositorioEmparejamientos, RepositorioEmparejamientos>();

// 5. Fuentes de Datos Externas (Implementaciones de Infraestructura)
builder.Services.AddScoped<IFuenteDatosExternaVacantes, FuenteDatosMockarooVacantes>();
builder.Services.AddScoped<IFuenteDatosExternaCandidatos, FuenteDatosMockarooCandidatos>();

// 6. Servicios de Dominio (Implementaciones)
builder.Services.AddScoped<IServicioEmparejamiento, ServicioEmparejamiento>();

// 7. Servicios para API Controllers y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Asegúrate de tener el paquete Swashbuckle.AspNetCore

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
// Si configuras HTTPS (launchSettings.json + dev-certs), puedes descomentar esto.
// app.UseHttpsRedirection();

// app.UseAuthorization(); // Descomenta si añades autenticación/autorización

// Mapear las peticiones a los Controllers
app.MapControllers();

// --- Ejecutar la Aplicación ---
app.Run();

// --- NOTA: Código de Ejemplo Eliminado ---
// El código por defecto de /weatherforecast y el record WeatherForecast
// ya no son necesarios y se asume que fueron eliminados.