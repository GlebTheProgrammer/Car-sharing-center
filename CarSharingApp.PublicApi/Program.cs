using CarSharingApp.Application.Interfaces;
using CarSharingApp.Application.Services;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.Infrastructure.MongoDB;
using CarSharingApp.Infrastructure.AzureKeyVault;
using CarSharingApp.Infrastructure.Options.Setup;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.ConfigureLogging((context, logging) =>
    {
        //logging.ClearProviders();
        logging.AddConfiguration(context.Configuration.GetSection("Logging"));
        logging.AddDebug();
        //logging.AddConsole();
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAzureKeyVaultAppsettingsValues(builder.Configuration);

    builder.Services.ConfigureOptions<JwtOptionsSetup>();

    builder.Services.AddMongo(builder.Configuration);
    builder.Services.AddMongoRepository<Vehicle>(builder.Configuration["MongoDbConfig:Collections:VehiclesCollectionName"] ?? "");
    builder.Services.AddSingleton<IVehicleService, VehicleService>();
    builder.Services.AddMongoRepository<Customer>(builder.Configuration["MongoDbConfig:Collections:CustomersCollectionName"] ?? "");
    builder.Services.AddSingleton<ICustomerService, CustomerService>();

    builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
    builder.Services.AddTransient<IJwtProvider, JwtProvider>();
    builder.Services.AddJwtBearerAuthentication(builder.Configuration);
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}

