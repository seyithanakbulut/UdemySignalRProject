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
        // baþlýða metoda  izin veriyor baðlanan izin verir
        builder.AllowAnyHeader().
        AllowAnyMethod().SetIsOriginAllowed((host) => true).
        AllowCredentials();

    });
});
builder.Services.AddSignalR();


builder.Services.AddDbContext<SignalRContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// buradan dep. aldýk
builder.Services.ContainerDependencies();
// validations iþlemi
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingValidation>();

// menutable product hatasýný bunla kaldýrýyoruz
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

app.UseCors("CorsPolicy");  // keyi çaðýrdýk burada 

app.MapHub<SignalRHub>("/signalrhub");



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


