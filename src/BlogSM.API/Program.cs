
using Asp.Versioning;

using BlogSM.API.Persistence;
using BlogSM.API.Services;

using Microsoft.EntityFrameworkCore;

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

    builder.Services.AddAutoMapper(typeof(Program).Assembly);

    builder.Services.AddScoped<BlogPostService>();

    builder.Services.AddDbContext<BlogSMDbContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSeeding(BlogSMDbSeed.SeedInitialData));
}


var app = builder.Build();
{
    app.MapControllers();
}

app.Run();
