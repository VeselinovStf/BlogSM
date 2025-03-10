using Asp.Versioning;

using BlogSM.API.Services;

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

    builder.Services.AddScoped< BlogPostService>();
}


var app = builder.Build();
{
    app.MapControllers();
}

app.Run();
