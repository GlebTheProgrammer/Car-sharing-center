using CarSharingApp.OptionsSetup;
using CarSharingApp.Order;
using CarSharingApp.Payment;
using CarSharingApp.Payment.StripeService;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Repository.LocalRepository;
using CarSharingApp.Repository.LocalRepository.Includes;
using CarSharingApp.Services;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Sessions setting
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Dependency injection is here
builder.Services.AddSingleton<IRepositoryManager, LocalRepositoryManager>();
builder.Services.AddSingleton<IPaymentSessionProvider, StripeSessionProvider>();
builder.Services.AddSingleton<IOrderProvider, CompleatedOrderProvider>();

builder.Services.AddScoped<IFileUploadService, LocalFileUploadService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ICurrentUserStatusProvider, CurrentUserStatusProviderService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

StripeConfiguration.ApiKey = "sk_test_51M6B0AGBXizEWSwDh5mkyk4o3DvKzmywGwJh7Fg2cpd9mxmhLiIPkARsFcvN3Yov0Qyshlqu8gITm3NGPPReXtbW00dvIu6aGa";

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
