using JwtIdentityApi.Data;
using JwtIdentityApi.Servicios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
{
    opciones.UseSqlServer("name=DefaultConnection");
});

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();
builder.Services.AddTransient<IUsuariosService,UsuariosService>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication().AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;

    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes
        (builder.Configuration["Token"]!)),
        ClockSkew=TimeSpan.Zero
    };

});

builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("admin", potilica => potilica.RequireClaim("admin"));
});


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.Run();
