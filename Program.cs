using System.Text;
using todo_app_backend.Data;
using todo_app_backend.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using todo_app_backend.Helpers;
using todo_app_backend.Services;
using todo_app_backend.Repositories.Contracts;
using todo_app_backend.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

var JwtSettings = builder.Configuration.GetSection("JwtSettings");

var credential = await OAuth.GetCredentialAsync();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionSQLServer")));
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
builder.Services.AddScoped<ITodoTaskService, TodoTaskService>();
builder.Services.AddScoped<ITodoSubtaskRepository, TodoSubtaskRepository>();
builder.Services.AddScoped<ITodoSubtaskService, TodoSubtaskService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option => {
        option.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuer = true,
            ValidIssuer = JwtSettings.GetSection("Issuer").Value,
            ValidateAudience = true,
            ValidAudience = JwtSettings.GetSection("Audience").Value,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero, 
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtSettings.GetSection("SecurityKey").Value!)
            ),
        };
    });
builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()  
              .AllowAnyMethod(); 
    });
});
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

app.Run();
