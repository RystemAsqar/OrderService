using DeliveryService.Api.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DeliveryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Ef_Postgres_Db")));


builder.Services.AddMassTransit(conf => {
    conf.SetKebabCaseEndpointNameFormatter();
    conf.SetInMemorySagaRepositoryProvider();
    
    var asb = typeof(Program).Assembly;
    
    conf.AddConsumers(asb);
    conf.AddSagaStateMachines(asb);
    conf.AddSagas(asb);
    conf.AddActivities(asb);
    
    conf.UsingRabbitMq((ctx, cfg) => 
    {
        cfg.Host("localhost", "/", host =>
        {
            host.Username("rystemasqar");
            host.Password("0055");
        });
        
        cfg.ConfigureEndpoints(ctx);
    });

});

var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
