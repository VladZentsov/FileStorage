using AutoMapper;
using BLL;
using BLL.Interfaces;
using BLL.Services;
using DAL.Entities;
using DAL.FileDBContext;
using DAL.Interfaces;
using DAL.RelatedDataLoader;
using DAL.Repositories;
using DAL.UoW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionStringSysDb = builder.Configuration.GetConnectionString("FileStorageDb");
//var connectionStringSysIdentityDb = builder.Configuration.GetConnectionString("SysIdentityDb");

//builder.Services.AddDbContext<SysDbContext>(options=>options.UseSqlServer(connectionStringSysDb, b=>b.MigrationsAssembly("DAL")));

builder.Services.AddScoped<IUserProfileRepo, UserProfileRepo>();
builder.Services.AddScoped<IUserIdentityRepo, UserIdentityRepo>();
builder.Services.AddScoped<IStFileRepo, StFileRepo>();

builder.Services.AddScoped<IUploadFileService, UploadFileService>();

builder.Services.AddIdentity<SysIdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<StorageDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IRelatedDataLoader, RelatedDataLoader>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();

builder.Services.AddDbContext<IStorageDbContext, StorageDbContext>(options =>
           options.UseSqlServer(connectionStringSysDb));

//builder.Services.AddAutoMapper(typeof(AutomapperProfile));

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutomapperProfile());

}).CreateMapper());

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})


// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
}); 


// Add services to the container.

builder.Services.AddControllers();


builder.Services.ConfigureSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
               new OpenApiSecurityScheme
                 {
                     Reference = new OpenApiReference
                     {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                     }
                 },
                 new string[] {}
         }
     });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("EnableCORS");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();