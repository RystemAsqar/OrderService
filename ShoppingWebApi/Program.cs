using MassTransit;
using Microsoft.EntityFrameworkCore;
using ShoppingWebApi.Data;
using ShoppingWebApi.Repositories;
using ShoppingWebApi.Services;
using ShoppingWebApi.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Register the repository and service
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<IMessagePublisherService, MessagePublisherService>();


// Register MassTransit 
builder.Services.AddMassTransit( conf => 
{
    conf.UsingRabbitMq((ctx, cfg) => 
    {
        cfg.Host("localhost", "/", host =>
        {
            host.Username("rystemasqar");
            host.Password("0055");
        });        
    });
});

// Configure Entity Framework with Npgsql and PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Ef_Postgres_Db")));


var app = builder.Build();

// Configure the HTTP request pipeline (middleware)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();