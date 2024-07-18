using ioLabs.Data;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In-memory database
builder.Services.AddDbContext<IoLabsContext>(options =>
    options.UseInMemoryDatabase("ioLabs"));

// Add OAuth2.0 authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://keycloak.stage.iolabs.ch/auth/realms/iotest/";
    options.ClientId = "test-api";
    options.ClientSecret = "vCDbLdj631sATkfujdg75j9WGzafryKf";
    options.ResponseType = OpenIdConnectResponseType.Code;

    options.SaveTokens = true;

    options.CallbackPath = "/signin-oidc";

    options.GetClaimsFromUserInfoEndpoint = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.OAuthClientId("test-api");
        c.OAuthClientSecret("vCDbLdj631sATkfujdg75j9WGzafryKf");
        c.OAuthRealm("https://keycloak.stage.iolabs.ch/auth/realms/iotest/");
        c.OAuthAppName("My API");

    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
