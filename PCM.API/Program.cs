using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PCM.API.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext - Auto-detect PostgreSQL (Render) or SQL Server (local)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Check if connection string contains "postgres" to use PostgreSQL
    if (!string.IsNullOrEmpty(connectionString) && 
        (connectionString.Contains("postgres", StringComparison.OrdinalIgnoreCase) ||
         connectionString.Contains("postgresql", StringComparison.OrdinalIgnoreCase)))
    {
        // PostgreSQL for Render.com production
        options.UseNpgsql(connectionString)
               .ConfigureWarnings(warnings => 
                   warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
    else
    {
        // SQL Server for local development
        options.UseSqlServer(connectionString)
               .ConfigureWarnings(warnings => 
                   warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
});

// Add Identity with custom password options (dễ demo)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();

// Add Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueDev", policy =>
    {
        policy.WithOrigins(
                // Local development
                "http://localhost:5173", 
                "http://localhost:3000",
                "http://localhost:8080",
                "http://frontend",
                "http://pcm-web",
                // Render.com production (update with your actual URLs)
                "https://pcm-web.onrender.com",
                "https://pcm-api.onrender.com",
                // Custom domain (update with your domain)
                "https://nguyenhoanganh.me",
                "https://www.nguyenhoanganh.me")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Auto-migrate database (important for Docker)
    context.Database.Migrate();
    
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    await SeedUsersAsync(userManager, roleManager, context);
}

// Configure the HTTP request pipeline.
// Enable Swagger in both Development and Production for documentation
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PCM API v1");
    c.RoutePrefix = "swagger";
});

// Skip HTTPS redirect in Docker (running behind Nginx proxy)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowVueDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint for Docker
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.Run();

// Seed method for users
async Task SeedUsersAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
{
    // Ensure roles exist
    string[] roles = { "Admin", "Treasurer", "Referee", "Member" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed users with correct passwords
    var seedUsers = new[]
    {
        new { Email = "admin@pcm.com", Password = "Admin@123", Role = "Admin", UserId = "admin-user-id-001" },
        new { Email = "treasurer@pcm.com", Password = "Treasurer@123", Role = "Treasurer", UserId = "treasurer-user-id-001" },
        new { Email = "referee@pcm.com", Password = "Referee@123", Role = "Referee", UserId = "referee-user-id-001" },
        new { Email = "member1@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-001" },
        new { Email = "member2@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-002" },
        new { Email = "member3@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-003" },
        new { Email = "member4@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-004" },
        new { Email = "member5@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-005" },
        new { Email = "member6@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-006" },
        new { Email = "member7@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-007" },
        new { Email = "member8@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-008" },
        new { Email = "member9@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-009" },
        new { Email = "member10@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-010" },
        new { Email = "member11@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-011" },
        new { Email = "member12@pcm.com", Password = "Member@123", Role = "Member", UserId = "member-user-id-012" }
    };

    foreach (var seedUser in seedUsers)
    {
        var existingUser = await userManager.FindByEmailAsync(seedUser.Email);
        if (existingUser == null)
        {
            // Check if user with this ID exists in database (from seed)
            var userFromDb = await context.Users.FirstOrDefaultAsync(u => u.Id == seedUser.UserId);
            if (userFromDb != null)
            {
                // Update password for existing seeded user
                var token = await userManager.GeneratePasswordResetTokenAsync(userFromDb);
                await userManager.ResetPasswordAsync(userFromDb, token, seedUser.Password);
            }
            else
            {
                // Create new user
                var user = new IdentityUser
                {
                    Id = seedUser.UserId,
                    UserName = seedUser.Email,
                    Email = seedUser.Email,
                    NormalizedUserName = seedUser.Email.ToUpper(),
                    NormalizedEmail = seedUser.Email.ToUpper(),
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, seedUser.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, seedUser.Role);
                }
            }
        }
        else
        {
            // User exists, reset password to ensure it's correct
            var token = await userManager.GeneratePasswordResetTokenAsync(existingUser);
            await userManager.ResetPasswordAsync(existingUser, token, seedUser.Password);
        }
    }
}
