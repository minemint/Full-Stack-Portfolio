using System.Text;
using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.SqlServer;

    var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection2"));
});



builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options  => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("JWT:ValidAudience").Value!,
        ValidIssuer = builder.Configuration.GetSection("JWT:ValidIssuer").Value!,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value!))
    };
});

builder.Services.AddCors(options => 
{
    options.AddPolicy("MultipleOrigins",
    policy =>
    {
        policy.WithOrigins(
            "*"
        )
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyMethod()
        .AllowAnyHeader();

    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    options =>
    {
        options.SupportNonNullableReferenceTypes();
        options.SwaggerDoc("v1", new() { Title = "Stock API with .NET 8 and PostgreSQL", Version = "v1" });

        options.AddSecurityDefinition("Bearer",  new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat= "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme."
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); 
}

app.UseHttpsRedirection();

app.UseCors("MultipleOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
