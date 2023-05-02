using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RonWeb.API.Enum;
using RonWeb.Core;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins(Environment.GetEnvironmentVariable(EnvVarEnum.ORIGINS.Description())!.Split(','))
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Scan(scan => scan
        .FromAssemblyOf<Program>()
        .AddClasses(classes =>
            classes.Where(t => t.Name.EndsWith("Helper", StringComparison.OrdinalIgnoreCase)))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                string key = Environment.GetEnvironmentVariable(EnvVarEnum.JWTKEY.Description())!;
                string issuer = Environment.GetEnvironmentVariable(EnvVarEnum.ISSUER.Description())!;
                string audience = Environment.GetEnvironmentVariable(EnvVarEnum.AUDIENCE.Description())!;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();
