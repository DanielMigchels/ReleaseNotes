using ReleaseNotes.API.Data;
using ReleaseNotes.API.Data.Models;
using ReleaseNotes.API.Options;
using ReleaseNotes.API.Services.Authentication;
using ReleaseNotes.API.Services.Bundle;
using ReleaseNotes.API.Services.Note;
using ReleaseNotes.API.Services.Project;
using ReleaseNotes.API.Services.Release;
using ReleaseNotes.API.Services.Seed;
using ReleaseNotes.API.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Network;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.TCPSink(builder.Configuration["Elastic:TcpSink"])
    .CreateLogger();

builder.Host.UseSerilog();

// ASP.NET
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Postgres
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"] ?? throw new Exception("Could not read JWT secret"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Options
builder.Services.AddOptions<JwtOptions>().Bind(builder.Configuration.GetSection("JwtOptions")).ValidateDataAnnotations().ValidateOnStart();

// Dependency Injection
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IProjectPdfGeneratorService, ProjectPdfGeneratorService>();
builder.Services.AddTransient<IReleaseService, ReleaseService>();
builder.Services.AddTransient<INoteService, NoteService>();
builder.Services.AddTransient<ISeedService, SeedService>();
builder.Services.AddTransient<IBundleService, BundleService>();
builder.Services.AddTransient<IBundlePdfGeneratorService, CoPilotBundlePdfGeneratorService>();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReleaseNotes.API", Version = "v1" });
    c.AddServer(new OpenApiServer { Url = "" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid token in the format 'Bearer {token}'"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            []
        }
    });
});

// Angular SPA
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot/ReleaseNotes.ui/browser/";
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

// ASP.NET
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Angular SPA
app.UseSpaStaticFiles();
#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

app.UseSerilogRequestLogging();

app.UseSpa(spa =>
{
    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    }
});

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
await db.Database.MigrateAsync();

if (app.Environment.IsDevelopment())
{
    var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
    try
    {
        await seedService.SeedFakeData();
    }
    catch { }
}

app.Run();