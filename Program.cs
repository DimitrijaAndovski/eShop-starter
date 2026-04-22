using eShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using eShop.Entities;
using System.Text;
using eShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using eShop.Interfaces;
using eShop.Repositories;
using eShop;
using eShop.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EShopContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("sqlite"));
    // options.UseNpgsql(builder.Configuration.GetConnectionString("postgres"));
});

builder.Services.AddAutoMapper(options =>
{
    options.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODA4MzUyMDAwIiwiaWF0IjoiMTc3Njg0MTIzMCIsImFjY291bnRfaWQiOiIwMTlkYjNmY2Q1MGU3NzVjOTFlNTA0ZDczYTNkYTJmYSIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa3BzenYyZG5kN2o4cWt3ajhjMnBqeG1mIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.Lm1u7CVV3vHJFo9cxqHwRf_QdplgHnuU7vSID2M_HDUBoYL4U8PgVYelMVFkDvvFtxB1EaVz4d5058Ish8f0zCjxrUUFMzOXODuiJUuAlktFyKTGQ-oqh51UtI8kurDE52Lki7xSEAgoW2GDKZuZ8RBLfoixiA39OKKfFBBhBfYApen_wwXqEEJcFAtLO5hihNIWdKB86NUZeSpZhc04gSeIlLvncn0_l2ttMrQG3JmHkCpRq6XCYPAsERtTs_aKaVfZNsy4aYPu-EjK19_c04yJm25oGI271vv-FiLSNgk-VUgDm8zf6jgsLcJFR2hvmw8rQ0PjHLfnR62BOZEpCA";
    options.AddProfile(new MappingProfiles());
});
// 1. Lägg till inloggningsinställningar (authentication)...
builder.Services.AddIdentityCore<User>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EShopContext>();

// Depency Injection...
// Registera vår TokenServer i dotnet's dependency lista...
builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers()  ;
// builder.Services.AddControllers(options =>
// {
//     // Skapa en generell regel(policy) som tvingar alla att vara inloggade...
//     var policy = new AuthorizationPolicyBuilder()
//         .RequireAuthenticatedUser()
//         .Build();

//     // Applicera regeln...
//     options.Filters.Add(new AuthorizeFilter(policy));

// });

// 2. Aktivera ett auktoriserings schema, dvs hur ska vi kontrollera användaren...
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("tokenSettings:tokenKey").Value))
        };
    });

// 6. Ett alternativt sätt att koppla behörighet i dotnet...
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireCorporateRights", policy => policy.RequireRole("Admin", "Manager"));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminRights", policy => policy.RequireRole("Admin"));

// 3. Aktivera behörighetskontroll...
builder.Services.AddAuthorization();

var app = builder.Build();

// 4. Använd användar inloggning i systemet...
app.UseAuthentication();

// 5. Använda behörighetskontroll...
app.UseAuthorization();

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    var seed = new SeedDatabase(userManager, roleManager);
    await seed.InitDb(app);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

app.Run();
