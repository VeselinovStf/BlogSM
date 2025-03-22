
using System.Net;
using System.Reflection;
using System.Text.Json;

using Asp.Versioning;

using BlogSM.API.Extensions;
using BlogSM.API.Persistence;

using Microsoft.EntityFrameworkCore;

using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

try
{
    logger.Info("BlogSM.API Starting...");

    var builder = WebApplication.CreateBuilder(args);
    {
        builder.Services.AddControllers();

        builder.Services.AddApiVersioning(options =>
          {
              options.DefaultApiVersion = new ApiVersion(1, 0);
              options.AssumeDefaultVersionWhenUnspecified = true;
              options.ReportApiVersions = true;
              options.ApiVersionReader = ApiVersionReader.Combine(
                  new UrlSegmentApiVersionReader(),
                  new HeaderApiVersionReader("X-Api-Version")
              );
          }).AddApiExplorer(options =>
          {
              options.GroupNameFormat = "'v'VVV";
              options.SubstituteApiVersionInUrl = true;
          });

        builder.Services.AddSwaggerGen();

        // Use NLog as the logging provider
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        builder.Services.AddAutoMapper(typeof(Program).Assembly);

        builder.Services.AddRepositories(Assembly.GetExecutingAssembly());
        builder.Services.AddServices(Assembly.GetExecutingAssembly());
        
        builder.Services.AddQueryStrategies(Assembly.GetExecutingAssembly());
        builder.Services.AddQueryStrategyFactories(Assembly.GetExecutingAssembly());

        builder.Services.AddDbContext<BlogSMDbContext>(options =>
            options.UseSqlServer(builder.Environment.IsProduction() ? 
                Environment.GetEnvironmentVariable("BlogSmAPIConnectionString") : builder.Configuration.GetConnectionString("DefaultConnection"))
            .UseSeeding(BlogSMDbSeed.SeedInitialData));
    }


    var app = builder.Build();
    {
        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
                   {
                       context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // HTTP 500
                       context.Response.ContentType = "application/json";
                       var errorResponse = new { success = false, message = "An unexpected error occurred." };

                       await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                   });
        });
    }

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
