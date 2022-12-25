using CarSharingApp.Application.Interfaces;
using CarSharingApp.Application.Services;
using CarSharingApp.Domain.Constants;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Infrastructure.MongoDB;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddMongo();
    builder.Services.AddMongoRepository<Vehicle>(MongoDbConstants.VEHICLES_COLLECTION_NAME);
    builder.Services.AddSingleton<IVehicleService, VehicleService>();
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

