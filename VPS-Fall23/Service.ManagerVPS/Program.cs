using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.AppSetting;
using Service.ManagerVPS.DTO.VNPay;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Extensions.Logic;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories;
using Service.ManagerVPS.Repositories.Interfaces;
using Service.ManagerVPS.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", build => build.AllowAnyMethod()
        .AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(hostName => true).Build());
});
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();


builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Config appsetting to model
builder.Services.Configure<FileManagementConfig>(builder.Configuration.GetSection("fileManagementAccessKey"));
builder.Services.Configure<VnPayConfig>(builder.Configuration.GetSection("vnPay"));
builder.Services.AddOptions();


//Add DBContext
builder.Services.AddDbContext<FALL23_SWP490_G14Context>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));

//AddSingleton 
builder.Services.AddSingleton<IGeneralVPS, GeneralVPS>();

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommuneRepository, CommuneRepository>();
builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IParkingZoneOwnerRepository, ParkingZoneOwnerRepository>();
builder.Services.AddScoped<IParkingZoneRepository, ParkingZoneRepository>();
builder.Services.AddScoped<IParkingTransactionRepository, ParkingTransactionRepository>();
builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
builder.Services.AddScoped<PaymentHub>();

//Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapHub<PaymentHub>("/payment");
app.MapControllers();
app.MapRazorPages();
app.UseExceptionHandler("/error");
app.UseSession();

app.Run();