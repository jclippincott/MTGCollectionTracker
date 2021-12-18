using MongoDB.Bson;
using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DTOs;

namespace TCGPlayerApiWrapper.DBAccess;

public class MtgSetService : DatabaseAccessService {
    public MtgSetService() { }

    public async Task<IList<MtgSetDTO>> CreateMtgSetDTOs(IList<MtgSet> newSets) {
        var db = Client.GetDatabase("MTGMetadata");
        IList<MtgSetDTO> newSetDTOs = newSets.Select(set => new MtgSetDTO() {
            Id = ObjectId.GenerateNewId().ToString(),
            GroupId = set.GroupId,
            Name = set.Name,
            Abbreviation = set.Abbreviation,
            IsSupplemental = set.IsSupplemental,
            ModifiedOn = set.ModifiedOn,
            PublishedOn = set.PublishedOn
        }).ToList();
        await db.GetCollection<MtgSetDTO>("Sets").InsertManyAsync(newSetDTOs);
        return newSetDTOs;
    }

    public async Task<MtgSetDTO> CreateMtgSetDTO(MtgSet newSet) {
        var db = Client.GetDatabase("MTGMetadata");
        var newSetDTO = new MtgSetDTO() {
            Id = ObjectId.GenerateNewId().ToString(),
            GroupId = newSet.GroupId,
            Name = newSet.Name,
            Abbreviation = newSet.Abbreviation,
            IsSupplemental = newSet.IsSupplemental,
            ModifiedOn = newSet.ModifiedOn,
            PublishedOn = newSet.PublishedOn
        };
        await db.GetCollection<MtgSetDTO>("Sets").InsertOneAsync(newSetDTO);
        return newSetDTO;
    }
}