////using Serilog;

//using SunnyVilla_VallaAPI.Loggin;

using Microsoft.EntityFrameworkCore;
using SunnyVilla_VallaAPI.Controllers;
using SunnyVilla_VallaAPI.Data;

var builder = WebApplication.CreateBuilder(args);

//////These is how to configure or logger using serilog- several mathod but we use simple way
////Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
////    .WriteTo.File("Log/VillaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
//////Next we add our logger to builder outside of console logger
////builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(Option =>
{
    //Option.ReturnHttpNotAcceptable = true; //these mean if our return type is no appropriable we return error type
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters(); //these way the support have be added to server accepting xml and json
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<ILogging, Logging>(); //adding customize services to our container using dependency injection

//Configure our database
builder.Services.AddDbContext<ApllicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
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

app.MapControllers();

app.Run();
