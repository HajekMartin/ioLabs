using ioLabs.Data;
using ioLabs.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Define the OAuth2 scheme that uses Authorization Code flow
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://keycloak.stage.iolabs.ch/auth/realms/iotest/protocol/openid-connect/auth"),
                TokenUrl = new Uri("https://keycloak.stage.iolabs.ch/auth/realms/iotest/protocol/openid-connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "Open ID" }
                }
            }
        }
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "openid" }
        }
    });
});

// In-memory database
builder.Services.AddDbContext<IoLabsContext>(options =>
    options.UseInMemoryDatabase("ioLabs"));

// SQLite
builder.Services.AddDbContext<IoLabsSQLContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("IoLabsContext")));

// Add OAuth2.0 JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = "https://keycloak.stage.iolabs.ch/auth/realms/iotest/";
    options.Audience = "account";

    options.RequireHttpsMetadata = false; // true

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});


builder.Services.AddHttpClient<TokenService>();

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
        //c.OAuthRealm("https://keycloak.stage.iolabs.ch/auth/realms/iotest/");
        c.OAuthAppName("My API");

        //c.OAuthUsePkce();  // Pøidejte, pokud je PKCE (Proof Key for Code Exchange) požadováno vaším serverem
        c.OAuthRealm("iotest");  // Toto by mìlo být ID realm, nikoli URL
        c.OAuthScopeSeparator(" ");  // Nastavte separátor používaný ve vašich scopes
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
