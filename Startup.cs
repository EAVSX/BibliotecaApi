using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BibliotecaApi.Datos;

// Este archivo define cómo se configuran los servicios y la configuración de la aplicación
namespace BibliotecaApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        // El constructor recibe la configuración (appsettings.json, variables de entorno, etc.)
        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // Aquí registramos los servicios que usará la aplicación
        public void ConfigureServices(IServiceCollection services)
        {
            // 1) Configuro el acceso a la base de datos usando Entity Framework y SQL Server
            //    La cadena de conexión viene de "DefaultConnection" en appsettings.json
            services.AddDbContext<BibliotecaContext>(delegate (DbContextOptionsBuilder options)
            {
                options.UseSqlServer(this._configuration.GetConnectionString("DefaultConnection"));
            });

            // 2) Registro los controladores para que funcionen como API (endpoints REST)
            services.AddControllers();

            // 3) Agrego Swagger para generar documentación automática de la API
            //    Swashbuckle genera un JSON y una interfaz web donde probamos los endpoints
            services.AddSwaggerGen(delegate (Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions c)
            {
                // Creo el objeto con el título y la versión de la API
                Microsoft.OpenApi.Models.OpenApiInfo info = new Microsoft.OpenApi.Models.OpenApiInfo();
                info.Title = "Biblioteca API";
                info.Version = "v1";
                // Registro el documento Swagger con la clave "v1"
                c.SwaggerDoc("v1", info);
            });
        }

        // Aquí definimos la tubería de procesamiento de peticiones HTTP
        public void Configure(IApplicationBuilder app)
        {
            // 1) Activo el enrutamiento para que la aplicación sepa cómo mapear URLs a controladores
            app.UseRouting();

            // 2) Habilito Swagger para exponer el JSON de especificación y la interfaz web
            app.UseSwagger();
            app.UseSwaggerUI(delegate (Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions c)
            {
                // Indico dónde encuentra el JSON que describe la API y le doy un nombre legible
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca API v1");
            });

            // 3) Indico que todos los endpoints de los controladores deben responder solicitudes
            app.UseEndpoints(delegate (Microsoft.AspNetCore.Routing.IEndpointRouteBuilder endpoints)
            {
                endpoints.MapControllers();
            });
        }
    }
}
