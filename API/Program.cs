using FSADProjectBackend.Autofac; using FSADProjectBackend.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");     
    });    
});


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5000"; // IdentityServer URL, Todo: Change to use environment variable
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
        
        // For debugging purposes only
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllers();

var corsPolicy = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
        policy => policy.WithOrigins("http://localhost:5173")  // Frontend URL, Todo: Change to use environment variable
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
        );
});
builder.Services.AddEndpointsApiExplorer(); // Enables API explorer for Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UPTS API", Version = "v1" });
});

// Register Autofac
AutofacRegister.Register(builder);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();   
    app.UseSwaggerUI();   
}

// Autoapply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PgDbContext>();

    var pendingMigrations = dbContext.Database.GetPendingMigrations();
    if (pendingMigrations.Any())
    {
        Console.WriteLine("Applying pending migrations...");
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied successfully.");
    }
    else
    {
        Console.WriteLine("No pending migrations found.");
    }
}

app.UseHttpsRedirection();
app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();