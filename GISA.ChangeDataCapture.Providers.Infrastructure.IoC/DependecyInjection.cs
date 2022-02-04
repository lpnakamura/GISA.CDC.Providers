using Amazon;
using Amazon.DynamoDBv2;
using GISA.ChangeDataCapture.Providers.Application.Contracts;
using GISA.ChangeDataCapture.Providers.Application.Mappings;
using GISA.ChangeDataCapture.Providers.Application.Notifications;
using GISA.ChangeDataCapture.Providers.Application.Services;
using GISA.ChangeDataCapture.Providers.Domain.Contracts;
using GISA.ChangeDataCapture.Providers.Infrastructure.Data.Configurations;
using GISA.ChangeDataCapture.Providers.Infrastructure.Data.Contracts;
using GISA.ChangeDataCapture.Providers.Infrastructure.Data.Repositories;
using GISA.ChangeDataCapture.MessageBroker.Extensions;
using GISA.ChangeDataCapture.MessageBroker.Models;
using GISA.ChangeDataCapture.Worker.Contracts;
using GISA.ChangeDataCapture.Worker.Extensions;
using GISA.ChangeDataCapture.Worker.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using ServiceStack;
using ServiceStack.Aws.DynamoDb;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GISA.ChangeDataCapture.Providers.Infrastructure.IoC
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ICloudConfiguration, DynamoDBConfiguration>();
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<IProviderService, Providerservice>();
            services.AddSingleton<IChangeDataCaptureObserver, ProviderObserverService>();

            return services;
        }

        public static IServiceCollection AddPocoDynamoDb(this IServiceCollection services)
        {
            services.AddSingleton(_ => CreateAmazonDynamoDb(
                services.BuildServiceProvider().GetService<ICloudConfiguration>()));

            services.AddSingleton<IPocoDynamo, PocoDynamo>();

            return services;
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProviderMapping));
        }

        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddChangeDataCaptureConsumer(new KafkaOptions
            {
                KafkaBootstrapServers = configuration.GetSection("Kafka:KafkaBootstrapServers").Value,
                KafkaGroupId = configuration.GetSection("Kafka:KafkaGroupId").Value,
                TopicName = configuration.GetSection("Kafka:TopicName").Value,
                MapperTo = GetKafkaMapperType(configuration)
            });

            return services;
        }

        public static IServiceCollection AddMessageBrokerNotification(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBrokerSimpleNotification(new AwsSimpleNotificationOptions
            {
                Region = configuration.GetSection("Aws:Region").Value,
                AccessKey = configuration.GetSection("Aws:AccessKey").Value,
                SecretKey = configuration.GetSection("Aws:SecretKey").Value,
                TopicArn = configuration.GetSection("Aws:SNS:TopicArn").Value
            });

            return services;
        }

        public static void AttachKafkaObserver(this IApplicationBuilder app)
        {
            var changeDataCaptureSubject = app.ApplicationServices.GetRequiredService<IChangeDataCaptureSubject>();
            var changeDataCaptureObserver = app.ApplicationServices.GetRequiredService<IChangeDataCaptureObserver>();
            changeDataCaptureSubject.Attach(changeDataCaptureObserver);
        }

        public static void AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GISA CDC Provider API",
                    Contact = new OpenApiContact
                    {
                        Name = "Luis Paulo Nakamura"
                    }
                });
            });
        }

        public static void AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options => options
                    .AddPolicy("CORS", option =>
                    {
                        option
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                       .WithOrigins(configuration.GetSection("ALLOWED_CORS").Get<string[]>());
                    }));
        }

        public static void UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors("CORS");
        }

        public static void AddSwaggerApplication(this IApplicationBuilder app, IApiVersionDescriptionProvider versionProvider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                foreach (var versionDescription in versionProvider.ApiVersionDescriptions)
                    swaggerUiOptions.SwaggerEndpoint($"../swagger/{versionDescription.GroupName}/swagger.json", versionDescription.GroupName.ToUpperInvariant());

                swaggerUiOptions.DocExpansion(DocExpansion.List);
            });
        }

        public static void UseSerilogExtension(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
        }

        private static Type GetKafkaMapperType(IConfiguration configuration)
        {
            var mapperAssembly = Assembly.LoadFile
                ($"{Path.Combine(AppContext.BaseDirectory, configuration.GetSection("Mapper:Namespace").Value)}.dll");

            return mapperAssembly.ExportedTypes.FirstOrDefault(exportedType =>
                exportedType.FullName.EndsWith(configuration.GetSection("Mapper:Model").Value));
        }

        private static IAmazonDynamoDB CreateAmazonDynamoDb(ICloudConfiguration cloudConfiguration)
        {
            return new AmazonDynamoDBClient(cloudConfiguration.GetAccessKey(),
                cloudConfiguration.GetSecretKey(), RegionEndpoint.GetBySystemName(cloudConfiguration.GetRegion()));
        }
    }
}
