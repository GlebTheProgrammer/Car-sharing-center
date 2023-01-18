using CarSharingApp.Application.Interfaces;
using CarSharingApp.Application.Services;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.Infrastructure.MongoDB;
using CarSharingApp.Infrastructure.AzureKeyVault;

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

    builder.Services.AddJwtBearerAuthentication(builder.Configuration);
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
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}

