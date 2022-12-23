using CarSharingApp.Application.Interfaces;
using CarSharingApp.Application.Services;
using CarSharingApp.Domain.Constants;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Infrastructure.MongoDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMongo();

//builder.Services.AddMongoRepository<Administrator>(MongoDbConstants.ADMINS_COLLECTION_NAME);
//builder.Services.AddMongoRepository<Customer>(MongoDbConstants.CUSTOMERS_COLLECTION_NAME);
//builder.Services.AddMongoRepository<CustomerCredentials>(MongoDbConstants.CUSTOMERS_CREDENTIALS_COLLECTION_NAME);
//builder.Services.AddMongoRepository<Payment>(MongoDbConstants.PAYMENTS_COLLECTION_NAME);
//builder.Services.AddMongoRepository<Rental>(MongoDbConstants.RENTALS_COLLECTION_NAME);
//builder.Services.AddMongoRepository<Review>(MongoDbConstants.REVIEWS_COLLECTION_NAME);
builder.Services.AddMongoRepository<Vehicle>(MongoDbConstants.VEHICLES_COLLECTION_NAME);

//builder.Services.AddSingleton<IVehicleService, VehicleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
