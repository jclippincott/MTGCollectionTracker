using MongoDB.Driver;

namespace TCGPlayerApiWrapper.DBAccess; 

public class DatabaseAccessService {
    private const string ConnectionString = "mongodb+srv://jclippincott:y69egN_K*!@mtg-collection.248ok.mongodb.net/CollectionTracker?retryWrites=true&w=majority";
    protected readonly IMongoClient Client;

    protected DatabaseAccessService() {
        Client = new MongoClient(ConnectionString);
    }
}