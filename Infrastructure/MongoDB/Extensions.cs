using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MongoDB.Driver;
using CarSharingApp.Domain.Constants;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Infrastructure.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

            services.AddSingleton(s =>
            {
                var mongoClient = new MongoClient(MongoDbConstants.CLOUD_CONNECTION_STRING);
                return mongoClient.GetDatabase(MongoDbConstants.DATABASE_NAME);
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
