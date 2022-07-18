using MongoDB.Driver;

namespace TCGPlayerApiWrapper.DBAccess;

public class DatabaseAccessService {
    protected readonly IMongoClient Client;

    private readonly string ConnectionString =
        "";

    protected DatabaseAccessService() {
        Client = new MongoClient(ConnectionString);
    }
}