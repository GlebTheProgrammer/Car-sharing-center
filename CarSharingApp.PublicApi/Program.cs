using CarSharingApp.Application.Interfaces;
using CarSharingApp.Application.Services;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Infrastructure.MongoDB;
using CarSharingApp.Infrastructure.AzureKeyVault;
using CarSharingApp.Infrastructure.MSSQL;
using CarSharingApp.Infrastructure.AzureAD;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.Infrastructure.Authentication.Options;

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
    builder.Services.AddSwaggerGen(config =>
    {
        config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Car Sharing Application Public API", Version = "v1" });
    });

    builder.Services.AddAzureKeyVaultAppsettingsValues(builder.Configuration);

    builder.Services.AddMongo(builder.Configuration);
    builder.Services.AddMongoRepository<Vehicle>(builder.Configuration["MongoDbConfig:Collections:VehiclesCollectionName"] ?? "");
    builder.Services.AddSingleton<IVehicleService, VehicleService>();
    builder.Services.AddMongoRepository<Customer>(builder.Configuration["MongoDbConfig:Collections:CustomersCollectionName"] ?? "");
    builder.Services.AddSingleton<ICustomerService, CustomerService>();

    builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();

    builder.Services.ConfigureOptions<JwtOptionsSetup>();
    builder.Services.AddTransient<IJwtProvider, JwtProvider>();
    builder.Services.AddJwtBearerAuthentication(builder.Configuration);
    //builder.Services.ConfigureAzureAD(builder.Configuration);

    builder.Services.AddMSSQLDBconnection(builder.Configuration);
}

var app = builder.Build();
{
    app.Logger.LogError(app.Environment.EnvironmentName);
    app.UseExceptionHandler("/error");

    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Sharing Application Public API");
    });

    //app.UseHttpsRedirection();
    app.UseAuthentication();



    app.MapControllers();
    app.Run();
}

