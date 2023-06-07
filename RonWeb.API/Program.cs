using AngleSharp;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RonWeb.API.Enum;
using RonWeb.Core;
using RonWeb.Database.MySql.RonWeb.DataBase;
using System.Reflection;
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

// Auto DI
builder.Services.Scan(scan => scan
        .FromAssemblyOf<Program>()
        .AddClasses(classes =>
            classes.Where(t => t.Name.EndsWith("Helper", StringComparison.OrdinalIgnoreCase)))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
);

// JWT Token
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
// 流量限制
//需要從載入配置檔案appsettings.json
builder.Services.AddOptions();
//需要儲存速率限制計算器和ip規則
builder.Services.AddMemoryCache();
//從appsettings.json中載入常規配置，IpRateLimiting與配置檔案中節點對應
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
//從appsettings.json中載入客戶端規則
builder.Services.Configure<ClientRateLimitPolicies>(builder.Configuration.GetSection("ClientRateLimitPolicies"));
//從appsettings.json中載入Ip規則
//builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

//注入計數器和規則儲存
builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
//配置（解析器、計數器金鑰生成器）
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

//swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RonWeb-API", Version = "v1" });
    // 指定 XML 檔案的路徑
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// db
builder.Services.AddDbContext<RonWebDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
//啟用客戶端IP限制速率
app.UseIpRateLimiting();
//啟用客戶端限制
app.UseClientRateLimiting();
app.Run();
