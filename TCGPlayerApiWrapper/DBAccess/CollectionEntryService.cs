using MongoDB.Bson;
using MongoDB.Driver;
using TCGPlayerApiWrapper.DBAccess.Interface;
using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DatabaseModels;

namespace TCGPlayerApiWrapper.DBAccess;

public class CollectionEntryService : ICollectionEntryService {
    private const string ConnectionString =
        "";

    private readonly IMongoClient _client;

    public CollectionEntryService() {
        _client = new MongoClient(ConnectionString);
    }

    public async Task<IList<bool>> UpsertCollectionEntries(IList<CollectionEntryDTO> newCollectionEntries) {
        IList<bool> upsertResults = new List<bool>();
        foreach (var collectionEntry in newCollectionEntries) {
            upsertResults.Add(await UpsertCollectionEntry(collectionEntry));
        }
        return upsertResults;
    }

    public async Task<bool> UpsertCollectionEntry(CollectionEntryDTO newCollectionEntry) {
        var db = _client.GetDatabase("MTGCollection");

        var collectionEntryFilter = new FilterDefinitionBuilder<CollectionEntryDTO>().Where(collectionEntryDto =>
            collectionEntryDto.SkuId == newCollectionEntry.SkuId);
        var updates = new UpdateDefinitionBuilder<CollectionEntryDTO>()
            .SetOnInsert(collectionEntryDto => collectionEntryDto.Id, ObjectId.GenerateNewId().ToString())
            .Set(collectionEntryDto => collectionEntryDto.Quantity, newCollectionEntry.Quantity)
            .Set(collectionEntryDto => collectionEntryDto.LastModifiedDate, DateTime.Now)
            .Set(collectionEntryDto => collectionEntryDto.LastValueRetrievalTime, DateTime.Now)
            .Set(collectionEntryDto => collectionEntryDto.UsdValue, newCollectionEntry.UsdValue);

        var upsertedEntry = await db.GetCollection<CollectionEntryDTO>("Cards").UpdateOneAsync(collectionEntryFilter, updates, new UpdateOptions {
            IsUpsert = true
        });
        return upsertedEntry.IsAcknowledged;
    }

    public async Task<int> QuantityOfSkuInCollection(int skuId) {
        var db = _client.GetDatabase("MTGCollection");
        var collectionEntryFilter = new FilterDefinitionBuilder<CollectionEntryDTO>().Where(collectionEntryDto =>
            collectionEntryDto.SkuId == skuId);
        var collectionEntry = (await db.GetCollection<CollectionEntryDTO>("Cards").FindAsync(collectionEntryFilter)).SingleOrDefault();
        if (collectionEntry == null || collectionEntry.Quantity <= 0) {
            return -1;
        }

        return collectionEntry.Quantity;
    }


}