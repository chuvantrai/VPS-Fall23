using Service.BrokerApi.Models;
using Service.BrokerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRabbitMQClient, RabbitMQClient>();
builder.Services.Configure<RabbitMQProfile>(builder.Configuration.GetSection("RabbitMQ"));
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(7002, listenOptions =>
    {
        listenOptions.UseHttps();
    });
    serverOptions.ListenLocalhost(5109);
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
app.UseExceptionHandler("/error");
app.MapControllers();

app.Run();
