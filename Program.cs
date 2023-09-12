using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rattrapage.Data;
using System.Text;
using Microsoft.OpenApi.Models; // Pour Swagger
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // Pour MySQL

var builder = WebApplication.CreateBuilder(args);

// Ajoutez les services nécessaires
builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21)))); // Remplacez par la version de votre serveur MySQL si elle est différente

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("a3R9j$#a2r8j$3j4k5l6m7n8o9p0q1r2s3t4u5v6w7x8y9z0A1B2C3D4E5F")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Ajoutez les services pour Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mon API", Version = "v1" });
});

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Ajoutez le middleware pour Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mon API v1");
});

app.UseRouting();
app.MapControllers();

app.Run();
