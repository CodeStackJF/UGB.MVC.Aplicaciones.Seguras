using UGB.MVC.Helper;
using UGB.MVC.Interfaces;
using UGB.MVC.Validations.UsersValidation;
using Microsoft.Extensions.Options;
using UGB.MVC.Aplicaciones.Seguras.Entities;
using Microsoft.EntityFrameworkCore;
using UGB.MVC.Aplicaciones.Seguras.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//agregamos la inyección de dependencias del validador de CreateUserDTO
builder.Services.AddValidationInjection();
builder.Services.AddRepositoryInjection();

builder.Services.AddSingleton<IConfigureOptions<SettingsBase>, MailSettings>();
builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddDbContext<StoreCTX>(options =>
    options.UseSqlite("Data Source=store.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
