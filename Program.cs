using UGB.MVC.Helper;
using UGB.MVC.Interfaces;
using UGB.MVC.Validations.UsersValidation;
using Microsoft.Extensions.Options;
using UGB.MVC.Aplicaciones.Seguras.Entities;
using Microsoft.EntityFrameworkCore;
using UGB.MVC.Aplicaciones.Seguras.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using UGB.MVC.Aplicaciones.Seguras.Policies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//agregamos la inyección de dependencias del validador de CreateUserDTO
builder.Services.AddValidationInjection();
builder.Services.AddRepositoryInjection();

//inyección de servicio de correo
builder.Services.AddSingleton<IConfigureOptions<SettingsBase>, MailSettings>();
builder.Services.AddSingleton<IEmailService, EmailService>();

//inyección de servicio de base de datos, store.db es el nombre del archivo sqlite a crear
builder.Services.AddDbContext<StoreCTX>(options =>
    options.UseSqlite("Data Source=store.db"));

//inyectamos el HttpContextAccessor para que sea accesible desde la politica
builder.Services.AddHttpContextAccessor();

//inyectamos la politica
builder.Services.AddSingleton<IAuthorizationHandler, ApiKeyPolicyHandler>();
builder.Services.AddAuthorization(options =>
{
    //aca se define el nombre de la politica que es necesaria para establecer en el controlador
    options.AddPolicy("API-KEY-POLICY", policy =>
        policy.Requirements.Add(new ApiKeyPolicyRequirement()));
});

//habilitamos la autenticación con cookies
builder.Services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                //Si el usuario no está autenticado, lo redirigira a /login
                options.LoginPath = "/Login";
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.Redirect("/Login");
                    return Task.CompletedTask;
                };
            });

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
