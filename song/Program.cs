using SONG.interfaces;
using SONG.Services;
using SONG.Models;
using user.Models;
using user.interfaces;
using user.Services;
using  Active.Interfaces;
using Active.Services;
using Generic.Interfaces;
using Generic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Token.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;

// Ensure JWT role claim stays as "role" (not mapped to ClaimTypes.Role)
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddUser();


builder.Services.AddActiveUser();
builder.Services.AddSong();
builder.Services.AddActiveUser();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false; // keep original claim types (e.g., "role")
    options.TokenValidationParameters =
    TokenService.GetTokenValidationParameters();
});

builder.Services.AddAuthorization(options =>
{
        options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("role", "admin"));
        options.AddPolicy("User", policy =>
        policy.RequireClaim("role", "user", "admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => Results.Redirect("/index.html")); 
app.MapControllers();
app.Run();
