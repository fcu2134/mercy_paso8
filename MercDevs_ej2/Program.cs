using MercDevs_ej2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//Para iniciar sesión y prohibir ingresos
using Microsoft.AspNetCore.Authentication.Cookies;

using System.Configuration;
//FIN
var builder = WebApplication.CreateBuilder(args);
//iintegro los servgicios que cree en la clase Email , sin esto no podria funcionarte ya que de aca saco la conexion 
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<Email>();
// Add services to the container.
builder.Services.AddControllersWithViews();
// coneccion a la bdd
builder.Services.AddDbContext<MercyDeveloperContext>(options =>
options.UseMySql(builder.Configuration.GetConnectionString("connection"),
Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.25-mariadb")));
//end bdd
//Para el login 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Ingresar";

        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

//FinParal ogin
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Ingresar}/{id?}");

app.Run();