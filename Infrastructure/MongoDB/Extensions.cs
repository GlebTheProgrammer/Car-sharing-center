using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MongoDB.Driver;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CarSharingApp.Infrastructure.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

            //services.AddSingleton(s =>
            services.AddTransient(s =>
            {
                MongoClient mongoClient = new MongoClient();

                if(hostEnvironment.IsDevelopment())
                    mongoClient = new MongoClient(configuration["ConnectionStrings:MongoDbLocal"]);
                else
                    mongoClient = new MongoClient(configuration["ConnectionStrings:MongoDbAtlasCloud"]);

                return mongoClient.GetDatabase(configuration["MongoDbConfig:DbName"]);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : Entity
        {
            //services.AddSingleton<IRepository<T>>(s =>
            services.AddTransient<IRepository<T>>(s =>
            {
                var database = s.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);
            });

            return services;
        }
    }
}
