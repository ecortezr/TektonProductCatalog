using FluentValidation;
using FluentValidation.AspNetCore;
using LazyCache;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Product.Api;
using Product.Api.Domain.Features.Products.Commands;
using Product.Api.Domain.Repositories;
using Product.Api.Domain.Validators;
using Product.Api.Infrastructure.HttpClient.MockApi;
using Product.Api.Infrastructure.Storage;
using Product.Infrastructure.HttpClient;
using Serilog;
using Serilog.Events;
using System.Reflection;

// Create Serilog logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs/response-time-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.PropertyNamingPolicy = null
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding SqLite
var connection = new SqliteConnection("DataSource=:memory:");
connection.Open();
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlite(connection)
);

// Adding FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBodyProductCommandValidator>();

// Adding MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateProductCommandHandler>());
// builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Adding LazyCache
builder.Services.AddLazyCache();

// Adding Serilog
builder.Host.UseSerilog();

// Adding external API Clients
var configuration = builder.Configuration;
builder.Services.AddClients(configuration);

// Adding http client implementations
builder.Services.AddScoped<IDiscountClient, DiscountClient>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var cache = scope.ServiceProvider.GetRequiredService<IAppCache>();
    
    var statusDictionary = new Dictionary<int, string>
    {
        { 1, "Active" },
        { 0, "Inactive" }
    };
    cache.Add("product-status", statusDictionary, DateTimeOffset.Now.AddMinutes(5));
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

// Add middleware to log the time of every request/response
// app.UseLogResponseTime();

app.UseAuthorization();

app.MapControllers();

app.Run();

Log.CloseAndFlush();