using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Play.Common.Settings;

namespace Play.Common
{
    public static class Extensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(x =>
            {
                var configuration = x.GetService<IConfiguration>();
                var serviceSettings = configuration.GetSection("ServiceSettings").Get<ServiceSettings>();

                var mongoDbSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });
            return services;
        }

        public static IServiceCollection AddMongoRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            return services;
        }
    }
}