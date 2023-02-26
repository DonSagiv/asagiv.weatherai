using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace asagiv.weatherai.service
{
    public class WeatherDataRepository
    {
        #region Fields
        private readonly ILogger? _logger;
        private readonly string? _connectionString;
        private readonly string? _dbName;
        private readonly string? _collectionName;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<WeatherData> _mongoCollection;
        #endregion

        #region Constructor
        public WeatherDataRepository(ILogger? logger = null)
        {
            _logger = logger;

            _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            _dbName = Environment.GetEnvironmentVariable("DB_NAME");
            _collectionName = Environment.GetEnvironmentVariable("COLLECTION_NAME");

            _logger?.Information("Mongo Connection String: {0}.", _connectionString);
            _logger?.Information("Mongo Database Name: {0}.", _dbName);
            _logger?.Information("Mongo Collection Name: {0}.", _collectionName);

            _mongoClient = new MongoClient(_connectionString);
            _mongoDatabase = _mongoClient.GetDatabase(_dbName);
            _mongoCollection = _mongoDatabase.GetCollection<WeatherData>(_collectionName);
        }
        #endregion

        #region Methods
        public Task InsertWeatherDataAsync(WeatherData weatherData)
        {
            _logger?.Information("Appending Weather Data to MongoDB.");

            return _mongoCollection.InsertOneAsync(weatherData);
        }
        #endregion
    }
}
