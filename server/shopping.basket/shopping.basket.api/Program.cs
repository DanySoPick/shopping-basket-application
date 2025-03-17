using shopping.basket.api.Profiler;
using shopping.basket.core;
using shopping.basket.shared.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region [ Logging ]
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
#endregion


builder.Services.AddCoreFeatures(builder.Configuration);

#region [ Cors ]
var corsOptions = builder.Configuration.GetSection(CorsOptions.Section).Get<CorsOptions>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
      policy =>
      {
          policy
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowAnyOrigin();

          if (corsOptions?.Origins is { Length: > 0 })
          {
              policy
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .WithOrigins(corsOptions.Origins);
          }
      });
});
#endregion

builder.Services.AddAutoMapper(typeof(MapperProfile));

//builder.Services.AddDatabase(builder.Configuration.GetConnectionString("DefaultConnection"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
