using System.Configuration;
using Bank.Service.Api.Auth;
using Bank.Service.Api.Data;
using Bank.Service.Api.Models;
using BankLibrary;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Сервис для работы контроллеров.
builder.Services.AddControllers();

// Добавление базы данных.
builder.Services.AddDbContext<DataBaseContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

// Документация Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options 
    => options.IncludeXmlComments("1.txt"));

builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Account>, AccountRepository>();
builder.Services.AddScoped<IRepository<HistoryOperation>, HistoryOperationRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = AuthOptions.Audience,
            ValidateLifetime = true,

            IssuerSigningKey =
                AuthOptions.GetSymmetricSecurityKey(builder.Configuration.GetSection("AppSettings:JWTToken").Value),
            ValidateIssuerSigningKey = true,
        };
    }).AddCookie(options => options.LoginPath = "/Users/login"); ;

builder.Services.Configure<IdentityOptions>(op =>
{
    op.Lockout.MaxFailedAccessAttempts = 10;
    op.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

PrepDb.PrepPopulation(app);
