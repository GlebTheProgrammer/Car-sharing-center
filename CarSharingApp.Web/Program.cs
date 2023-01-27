using CarSharingApp.Login;
using CarSharingApp.Login.Authentication;
using CarSharingApp.OptionsSetup;
using CarSharingApp.Payment;
using CarSharingApp.Payment.StripeService;
using CarSharingApp.Services;
using CarSharingApp.Services.Interfaces;
using Stripe;
using CarSharingApp.Middlewares;
using CarSharingApp.Repository.MongoDbRepository;
using CarSharingApp.Web.Clients;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Clients.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging((context, logging) =>
{
    //logging.ClearProviders();
    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
    logging.AddDebug();
    //logging.AddConsole();
});

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IVehicleServicePublicApiClient, VehicleServicePublicApiClient>();
builder.Services.RegisterNewHttpClients("VehiclesAPI", builder.Configuration);
builder.Services.AddSingleton<ICustomerServicePublicApiClient, CustomerServicePublicApiClient>();
builder.Services.RegisterNewHttpClients("CustomersAPI", builder.Configuration);
builder.Services.AddSingleton<IAuthorizationServicePublicApiClient, AuthorizationServicePublicApiClient>();
builder.Services.RegisterNewHttpClients("AuthorizationAPI", builder.Configuration);
builder.Services.AddSingleton<IAccountServicePublicApiClient, AccountServicePublicApiClient>();
builder.Services.RegisterNewHttpClients("AccountsAPI", builder.Configuration);
builder.Services.AddSingleton<IAzureADPublicApiClient, AzureADPublicApiClient>();
builder.Services.RegisterNewHttpClients("AzureActiveDirectoryAPI", builder.Configuration);

builder.Services.RegisterAzureBlobStorageClient(builder.Configuration);















// Sessions setting
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Dependency injection is here
builder.Services.AddSingleton<IPaymentSessionProvider, StripeSessionProvider>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<IJwtProvider, JwtProvider>();

// JWT options configuration setup is here
builder.Services.ConfigureOptions<JwtOptionsSetup>();

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

//app.UseHttpsRedirection();

app.UseSession();

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
