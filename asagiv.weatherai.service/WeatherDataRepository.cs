using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace asagiv.weatherai.service
{
    public class WeatherDataRepository
    {
        #region Fields
        private readonly string? _connectionString;
        private readonly string? _dbName;
        private readonly string? _collectionName;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<WeatherData> _mongoCollection;
        #endregion

        #region Constructor
        public WeatherDataRepository()
        {
            _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            _dbName = Environment.GetEnvironmentVariable("DB_NAME");
            _collectionName = Environment.GetEnvironmentVariable("COLLECTION_NAME");

            _mongoClient = new MongoClient(_connectionString);
            _mongoDatabase = _mongoClient.GetDatabase(_dbName);
            _mongoCollection = _mongoDatabase.GetCollection<WeatherData>(_collectionName);
        }
        #endregion

        #region Methods
        public Task InsertWeatherDataAsync(WeatherData weatherData)
        {
            return _mongoCollection.InsertOneAsync(weatherData);
        }
        #endregion
    }
}
