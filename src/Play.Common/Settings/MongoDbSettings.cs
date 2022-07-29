namespace Play.Common.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; init; }
        public long Port { get; init; }

        public string ConnectionString => $"mongodb://{Host}:{Port}";

    }
}