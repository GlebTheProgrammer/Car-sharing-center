using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Constants;
using CarSharingApp.Application.Services;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.Infrastructure.MongoDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddMongo();
    builder.Services.AddMongoRepository<Vehicle>(MongoDbConstants.VEHICLES_COLLECTION_NAME);
    builder.Services.AddSingleton<IVehicleService, VehicleService>();
    builder.Services.AddMongoRepository<Customer>(MongoDbConstants.CUSTOMERS_COLLECTION_NAME);
    builder.Services.AddSingleton<ICustomerService, CustomerService>();

    builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
    builder.Services.AddTransient<IJwtProvider, JwtProvider>();

    var key = Encoding.UTF8.GetBytes(JwtAuthorizationConstants.SECRET_KEY);
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
            ValidIssuers = new string[] { JwtAuthorizationConstants.ISSUER },
            ValidAudiences = new string[] { JwtAuthorizationConstants.AUDIENCE },
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
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

