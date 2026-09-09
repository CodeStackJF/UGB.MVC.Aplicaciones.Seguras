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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Text;
using UGB.MVC.Aplicaciones.Seguras.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//agregamos la inyección de dependencias del validador de CreateUserDTO
builder.Services.AddValidationInjection();

//inyectamos los repositorios de base de datos
builder.Services.AddRepositoryInjection();

//inyección de servicio de correo
builder.Services.AddSingleton<IConfigureOptions<SettingsBase>, MailSettings>();
builder.Services.AddSingleton<IEmailService, EmailService>();

//inyección de servicio de base de datos, store.db es el nombre del archivo sqlite a crear
builder.Services.AddDbContext<StoreCTX>(options =>
    options.UseSqlite("Data Source=store.db"));

//inyectamos el HttpContextAccessor para que sea accesible desde la politica
builder.Services.AddHttpContextAccessor();

var JWT_TOKEN = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWT_TOKEN")!);

 builder.Services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.RequireAuthenticatedSignIn = true;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(JWT_TOKEN),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new 
                        {
                            message = "No se encontro el token de autenticación.",
                            statusCode = 401
                        });
                        
                        return context.Response.WriteAsync(result);
                    }
                };
            });

//inyectamos la politica
builder.Services.AddSingleton<IAuthorizationHandler, ApiKeyPolicyHandler>();
builder.Services.AddAuthorization(options =>
{
    //aca se define el nombre de la politica que es necesaria para establecer en el controlador
    options.AddPolicy("API-KEY-POLICY", policy =>
        policy.Requirements.Add(new ApiKeyPolicyRequirement()));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
