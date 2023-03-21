using CarSharingApp.Web.Clients;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Clients.Extensions;
using CarSharingApp.Web.OptionsSetup;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddSeq();

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
builder.Services.AddSingleton<IStripePlatformPublicApiClient, StripePlatformPublicApiClient>();
builder.Services.RegisterNewHttpClients("PaymentsAPI", builder.Configuration);
builder.Services.AddSingleton<IRentalServicePublicApiClient, RentalServicePublicApiClient>();
builder.Services.RegisterNewHttpClients("RentalsAPI", builder.Configuration);

builder.Services.RegisterAzureBlobStorageClient(builder.Configuration);
builder.Services.AddJwtBearerAuthentication(builder.Configuration);

// Sessions setting
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

app.Logger.LogInformation("ClIENT APP CREATED...");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/home/error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");
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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/");

app.Logger.LogInformation("LAUNCHING CLIENT APPLICATION");

app.Run();
