using GISA.ChangeDataCapture.Providers.Infrastructure.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GISA.ChangeDataCapture.Providers.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure();
            services.AddAutoMapper();
            services.AddKafkaConsumer(Configuration);
            services.AddMessageBrokerNotification(Configuration);
            services.AddSwaggerDocumentation(Configuration);
            services.AddPocoDynamoDb();
            services.ConfigureCors(Configuration);
            services.AddApiVersion();
            services.AddHealthChecks();
            services.AddResponseCompression();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionProvider)
        {
            if (env.IsDevelopment())            
                app.UseDeveloperExceptionPage();

            app.UseAppCors();
            app.UseHealthChecks("/health");
            app.AddSwaggerApplication(versionProvider);
            app.AttachKafkaObserver();
            app.UseApiVersioning();
            app.UseSerilogExtension();
            app.UseRouting();
            app.UseResponseCompression();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
