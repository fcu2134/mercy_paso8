using MercDevs_ej2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuración de la conexión a la base de datos
builder.Services.AddDbContext<MercyDeveloperContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("connection"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.25-mariadb")));

// Configuración de autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Ingresar";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

// Configuración de Rotativa
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);
builder.Services.AddSingleton<IRotativaConfiguration>(new RotativaConfiguration
{
    WkhtmltopdfPath = @"C:\Users\f_mrt\OneDrive\Escritorio\proyectos en net-\mer_p8\MercDevs_ej2\wwwroot\rotativa\wkhtmltopdf.exe"
});

var app = builder.Build();

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

// Configuración de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Ingresar}/{id?}");

app.Run();
