using GenerateTicket.Consumers;
using GenerateTicket.Models;
using GenerateTicket.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register MassTransit 
builder.Services.AddMassTransit(cfg => {
    cfg.AddBus(provider => MessageBrokers.RabbitMQ.ConfigureBus(provider));
    cfg.AddConsumer<GenerateTicketConsumer>();
    cfg.AddConsumer<CancelSendingEmailConsumer>();
});

var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContextPool<AppDbContext>(db => db.UseSqlServer(connectionString));

builder.Services.AddScoped<ITicketInfoService, TicketInfoService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
