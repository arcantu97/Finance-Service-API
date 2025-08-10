using FinanceService.Application.Transactions;
using FinanceService.Data;
using FinanceService.Domain.Repositories;
using FinanceService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft", LogLevel.Warning); // Solo warnings y errores de Microsoft.*
builder.Logging.AddFilter("System", LogLevel.Warning);    // Igual para System.*
builder.Logging.AddFilter("FinanceService-API", LogLevel.Information); 
builder.Services.AddDbContext<FinanceContext>(options => options.UseSqlite("Data Source=finance.db"));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Mantener nombres de propiedades como están
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()); 
});

var runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

if (string.Equals(runningInContainer, "true", StringComparison.OrdinalIgnoreCase))
{
    // En Docker/Azure: HTTP en 80 (ingress/proxy hace TLS si aplica)
    builder.WebHost.UseUrls("http://0.0.0.0:80");
}
else if (builder.Environment.IsDevelopment())
{
    // En local: HTTP 8080 (y opcional HTTPS 8443 si confías cert dev)
    builder.WebHost.UseUrls("http://localhost:8080;https://localhost:8443");
}

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "FinanceService API",
        Version = "v1",
        Description = "API para gestionar gastos, ingresos y predicciones financieras personales",
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Opcional: incluir comentarios XML para documentar métodos y modelos
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionsService, TransactionsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FinanceContext>();
    db.Database.Migrate();
}

app.UseSwagger();

app.UseSwaggerUI();

app.MapControllers();

app.Run();