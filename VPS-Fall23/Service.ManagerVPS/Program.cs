using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Extensions.Logic;
using Service.ManagerVPS.Repositories;
using Service.ManagerVPS.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AddSingleton 
builder.Services.AddSingleton<IGeneralVPS, GeneralVPS>();

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler("/error");
app.Run();