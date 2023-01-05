using CarSharingApp.Login;
using CarSharingApp.Login.Authentication;
using CarSharingApp.OptionsSetup;
using CarSharingApp.Payment;
using CarSharingApp.Payment.StripeService;
using CarSharingApp.Services;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Stripe;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CarSharingApp.Middlewares;
using CarSharingApp.Repository.MongoDbRepository;
using CarSharingApp.Web.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddSingleton<IVehicleServicePublicApiClient, VehicleServicePublicApiClient>();
builder.Services.RegisterNewHttpClients("VehiclesAPI", builder.Configuration);










// Sessions setting
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Dependency injection is here
builder.Services.AddSingleton<IPaymentSessionProvider, StripeSessionProvider>();

builder.Services.AddScoped<IFileUploadService, LocalFileUploadService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<IJwtProvider, JwtProvider>();

// JWT options configuration setup is here
builder.Services.ConfigureOptions<JwtOptionsSetup>();

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidIssuers = new string[] { builder.Configuration["Jwt:Issuer"] },
        ValidAudiences = new string[] { builder.Configuration["Jwt:Audience"] },
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// DB configuration section
builder.Services.Configure<CarSharingDatabaseSettings>(
    builder.Configuration.GetSection("CarSharingLocalDB"));
builder.Services.AddSingleton<MongoDbService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSession();

app.Use(async (context, next) =>
{
    var JWToken = context.Session.GetString("JWToken");
    if (!string.IsNullOrEmpty(JWToken))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
    }
    await next();
});
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseAuthorization();

StripeConfiguration.ApiKey = "sk_test_51M6B0AGBXizEWSwDh5mkyk4o3DvKzmywGwJh7Fg2cpd9mxmhLiIPkARsFcvN3Yov0Qyshlqu8gITm3NGPPReXtbW00dvIu6aGa";

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
