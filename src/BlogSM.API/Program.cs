using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

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


var app = builder.Build();
app.Run();
