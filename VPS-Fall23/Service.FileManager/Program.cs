using VPS.MinIO.API.Configurations;
using VPS.MinIO.Repository.AutoMapperProfile;
using VPS.MinIO.Repository.MinIO.Bucket;
using VPS.MinIO.Repository.MinIO.Object.External;
using VPS.MinIO.Repository.MinIO.Object;
using VPS.MinIO.Repository;
using VPS.MinIO.BusinessObjects.AppSetting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.OperationFilter<SwaggerHeader>();
});
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));
builder.Services.AddOptions();
builder.Services
    .UseMinvoiceMinIORepository<IBucketRepository, BucketRepository>()
    .UseMinvoiceMinIORepository<IObjectRepository, ObjectRepository>()
    .UseMinvoiceMinIORepository<IExternalRepository, ExternalRepository>();
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
