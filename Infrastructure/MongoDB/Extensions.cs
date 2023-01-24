using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MongoDB.Driver;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Primitives;
using Microsoft.Extensions.Configuration;

namespace CarSharingApp.Infrastructure.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            
            services.AddSingleton(s =>
            {
                MongoClient mongoClient = new MongoClient();

                mongoClient = new MongoClient(configuration["ConnectionStrings:MongoDbLocal"]);
                //mongoClient = new MongoClient(configuration["ConnectionStrings:MongoDbAtlasCloud"]);

                return mongoClient.GetDatabase(configuration["MongoDbConfig:DbName"]);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : Entity
        {
            services.AddSingleton<IRepository<T>>(s =>
            {
                var database = s.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);
            });

            return services;
        }
    }
}
