using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using SignalR.DataAccessLayer.Concrete;
using SignalR.EntityLayer.Entities;

var builder = WebApplication.CreateBuilder(args);

//Örneðin, sadece kimlik doðrulamasý yapýlmýþ kullanýcýlarýn belirli bir API'ye eriþmesine izin vermek için bu politika kullanýlabilir.
var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();


// SignalR ve identity context kýsýmlarý yapýldý
builder.Services.AddDbContext<SignalRContext>();
builder.Services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<SignalRContext>();

builder.Services.AddHttpClient();   
// Add services to the container.
builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
});

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Login/Index";
});

var app = builder.Build();

app.UseStatusCodePages(async x =>
{
    if (x.HttpContext.Response.StatusCode == 404)
    {
        x.HttpContext.Response.Redirect("/Error/NotFound404Page/");
    }
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=About}/{action=Index}/{id?}");

app.Run();
