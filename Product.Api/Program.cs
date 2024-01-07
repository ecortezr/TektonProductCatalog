using FluentValidation;
using FluentValidation.AspNetCore;
using LazyCache;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Product.Api.Infrastructure;
using Product.Api.Validators;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductCommandValidator>();

// Adding MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Adding LazyCache
builder.Services.AddLazyCache();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
