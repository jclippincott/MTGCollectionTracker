using MongoDB.Bson;
using MongoDB.Driver;
using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DatabaseModels;

namespace TCGPlayerApiWrapper.DBAccess;

public class SetService : DatabaseAccessService {
    private const string ConnectionString =
        "";

    private readonly IMongoClient _client;

    public SetService() {
        _client = new MongoClient(ConnectionString);
    }

    public async Task<IList<SetDTO>> CreateSetDTOs(IList<Set> newSets) {
        var db = Client.GetDatabase("MTGMetadata");
        IList<SetDTO> newSetDTOs = newSets.Select(set => new SetDTO {
            Id = ObjectId.GenerateNewId().ToString(),
            GroupId = set.GroupId,
            Name = set.Name,
            Abbreviation = set.Abbreviation,
            IsSupplemental = set.IsSupplemental,
            ModifiedOn = set.ModifiedOn,
            PublishedOn = set.PublishedOn
        }).ToList();
        await db.GetCollection<SetDTO>("Sets").InsertManyAsync(newSetDTOs);
        return newSetDTOs;
    }

    public async Task<SetDTO> CreateSetDTO(Set newSet) {
        var db = Client.GetDatabase("MTGMetadata");
        var newSetDTO = new SetDTO {
            Id = ObjectId.GenerateNewId().ToString(),
            GroupId = newSet.GroupId,
            Name = newSet.Name,
            Abbreviation = newSet.Abbreviation,
            IsSupplemental = newSet.IsSupplemental,
            ModifiedOn = newSet.ModifiedOn,
            PublishedOn = newSet.PublishedOn
        };
        await db.GetCollection<SetDTO>("Sets").InsertOneAsync(newSetDTO);
        return newSetDTO;
    }

    public async Task<SetDTO> UpdateSet(SetDTO set) {
        var db = _client.GetDatabase("MTGMetadata");
        var setFilter = new FilterDefinitionBuilder<SetDTO>().Where(setDto =>
            setDto.GroupId == set.GroupId);
        var updates = new UpdateDefinitionBuilder<SetDTO>()
            .Set(setDto => setDto.ModifiedOn, set.ModifiedOn)
            .Set(setDto => setDto.SetSymbolImageUrl, set.SetSymbolImageUrl);

        var foundSet = await db.GetCollection<SetDTO>("Sets").FindAsync(setFilter);
        var updatedSet = await db.GetCollection<SetDTO>("Sets").FindOneAndUpdateAsync(setFilter, updates);
        return updatedSet;
    }

    public async Task<UpdateResult> UpdateSets(IList<Set> sets) {
        var db = _client.GetDatabase("MTGMetadata");
        var setIds = sets.Select(set => set.GroupId);
        var setFilter = new FilterDefinitionBuilder<SetDTO>().Where(setDto =>
            setIds.Contains(setDto.GroupId));

        var updates = new UpdateDefinitionBuilder<SetDTO>()
            .Set(setDto => setDto.ModifiedOn, sets.First().ModifiedOn)
            .Set(setDto => setDto.SetSymbolImageUrl, sets.First().SetSymbolImageUrl);

        foreach (var set in sets)
            updates.Set(setDto => setDto.ModifiedOn, set.ModifiedOn)
                .Set(setDto => setDto.SetSymbolImageUrl, set.SetSymbolImageUrl);

        var setUpdateResult = await db.GetCollection<SetDTO>("Sets").UpdateManyAsync(setFilter, updates);
        return setUpdateResult;
    }

    public IList<SetDTO> GetSetsByIds(IList<int> setIds) {
        var db = Client.GetDatabase("MTGMetadata");
        var matchingSets = db.GetCollection<SetDTO>("Sets").AsQueryable()
            .Where(set => setIds.Contains(set.GroupId));
        return matchingSets.ToList();
    }
}