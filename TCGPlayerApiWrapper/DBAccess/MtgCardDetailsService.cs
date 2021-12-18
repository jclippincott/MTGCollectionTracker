using MongoDB.Bson;
using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DTOs;

namespace TCGPlayerApiWrapper.DBAccess;

public class MtgCardDetailsService : DatabaseAccessService {
    public MtgCardDetailsService() { }

    public async Task<IList<MtgCardDetailsDTO>> CreateMtgCardDetailsDTOs(IList<MtgCardDetails> newCardDetails) {
        var db = Client.GetDatabase("MTGMetadata");
        IList<MtgCardDetailsDTO> newCardDetailsDTOs = newCardDetails.Select(cardDetails => new MtgCardDetailsDTO() {
            Id = ObjectId.GenerateNewId().ToString(),
            ProductId = cardDetails.ProductId,
            Name = cardDetails.Name,
            CleanName = cardDetails.CleanName,
            GroupId = cardDetails.GroupId,
            SKUs = cardDetails.SKUs
        }).ToList();
        await db.GetCollection<MtgCardDetailsDTO>("CardDetails").InsertManyAsync(newCardDetailsDTOs);
        return newCardDetailsDTOs;
    }

    public async Task<MtgCardDetailsDTO> CreateMtgSetDTO(MtgCardDetailsDTO cardDetails) {
        var db = Client.GetDatabase("MTGMetadata");
        var newCardDetailsDTO = new MtgCardDetailsDTO() {
            Id = ObjectId.GenerateNewId().ToString(),
            ProductId = cardDetails.ProductId,
            Name = cardDetails.Name,
            CleanName = cardDetails.CleanName,
            GroupId = cardDetails.GroupId,
            SKUs = cardDetails.SKUs
        };
        await db.GetCollection<MtgCardDetailsDTO>("CardDetails").InsertOneAsync(newCardDetailsDTO);
        return newCardDetailsDTO;
    }
}