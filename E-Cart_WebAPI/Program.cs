using E_Cart_WebAPI.Data;
using E_Cart_WebAPI.Middleware;
using E_Cart_WebAPI.Models;
using E_Cart_WebAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var _logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("C:\\dev\\dotNetProject\\logs\\ECartAPILog-.log", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer("Server=.\\SQLExpress;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True",
    new MSSqlServerSinkOptions
    {
        TableName = "EcartAPILog",
        SchemaName = "dbo",
        AutoCreateSqlTable = true
    })
    .WriteTo.MSSqlServer("Server=.\\SQLExpress;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True",
    new MSSqlServerSinkOptions
    {
        TableName = "ApplicationAuditTrail",
        SchemaName = "dbo",
        AutoCreateSqlTable = true
    })
    .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Logging.AddSerilog(_logger);
builder.Services.AddDbContext<NorthwindContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "New Swagger",
        Version = "v1",
        Description = "New Swagger Document"
    });


    var filename = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
    var filepath = Path.Combine(AppContext.BaseDirectory, filename);
    options.IncludeXmlComments(filepath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
