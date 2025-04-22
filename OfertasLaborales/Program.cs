// --- PASO 1: Añadir Usings Necesarios ---
// Asegúrate de que los namespaces coincidan con los de tus proyectos
using Aplicacion.Consultas; // Para MediatR (usando una clase de ejemplo como ConsultaContarDatosExternos)
using Dominio.Interfaces;
using Infraestructura.FuentesDatos;
using MediatR; // Necesario para AddMediatR

var builder = WebApplication.CreateBuilder(args);

// --- PASO 2: Añadir Servicios al Contenedor (Dependency Injection) ---

// 1. HttpClientFactory (Necesario para nuestras Fuentes de Datos)
builder.Services.AddHttpClient();

// 2. MediatR (Para el patrón CQRS/Mediator)
// Reemplaza 'ConsultaContarDatosExternos' con cualquier clase que esté en tu proyecto 'Aplicacion'
// o crea una clase pública vacía 'AssemblyReference' en Aplicacion y usa typeof(Aplicacion.AssemblyReference)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ConsultaContarDatosExternos>());

// 3. Registra tus implementaciones de fuentes de datos (Infraestructura)
// Usamos AddScoped porque HttpClientFactory es seguro para usar en Scoped/Transient y así
// compartimos la misma instancia dentro de una misma petición HTTP si fuera necesario.
builder.Services.AddScoped<IFuenteDatosExternaVacantes, FuenteDatosMockarooVacantes>();
builder.Services.AddScoped<IFuenteDatosExternaCandidatos, FuenteDatosMockarooCandidatos>();

// 4. Servicios para API Controllers (En lugar de AddOpenApi/MapOpenApi para Minimal API)
builder.Services.AddControllers(); // Habilita el uso de Controllers
builder.Services.AddEndpointsApiExplorer(); // Necesario para que Swagger descubra los endpoints de los controllers
builder.Services.AddSwaggerGen(); // Genera la documentación Swagger

// --- PASO 3: Construir la Aplicación ---
var app = builder.Build();

// --- PASO 4: Configurar el Pipeline de Peticiones HTTP ---

// Habilitar Swagger solo en Desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Genera el archivo swagger.json
    app.UseSwaggerUI(); // Muestra la UI interactiva de Swagger
}

// Middlewares estándar
app.UseHttpsRedirection();

// app.UseAuthorization(); // Descomenta si necesitas autorización más adelante

// Mapear las peticiones a los Controllers que definiste en API > Controladores
app.MapControllers();

// --- PASO 5: Ejecutar la Aplicación ---
app.Run();

// --- PASO 6: Eliminar Código de Ejemplo Minimal API (Opcional pero Recomendado) ---
// Ya no necesitamos el endpoint /weatherforecast ni el record WeatherForecast
// El código relacionado con 'summaries', 'app.MapGet("/weatherforecast", ...)' y 'record WeatherForecast'
// puede ser eliminado de este archivo para mayor claridad.