using FluentValidation;
using SignalR.BusinessLayer.Abstract;
using SignalR.BusinessLayer.Concrete;
using SignalR.BusinessLayer.Container;
using SignalR.BusinessLayer.ValidationRules.BookingValidations;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.EntityFramework;
using SignalRApi.Hubs;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt=>
{
    opt.AddPolicy("CorsPolicy",builder=>
    {
        // ba�l��a metoda  izin veriyor ba�lanan izin verir
        builder.AllowAnyHeader().
        AllowAnyMethod().SetIsOriginAllowed((host) => true).
        AllowCredentials();

    });
});
builder.Services.AddSignalR();


builder.Services.AddDbContext<SignalRContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// buradan dep. ald�k
builder.Services.ContainerDependencies();
// validations i�lemi
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingValidation>();

// menutable product hatas�n� bunla kald�r�yoruz
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");  // keyi �a��rd�k burada 

app.MapHub<SignalRHub>("/signalrhub");



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


