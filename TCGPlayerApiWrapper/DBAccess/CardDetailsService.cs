using MongoDB.Bson;
using MongoDB.Driver;
using TCGPlayerApiWrapper.DBAccess.Interface;
using TCGPlayerApiWrapper.Enums;
using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DatabaseModels;

namespace TCGPlayerApiWrapper.DBAccess;

public class CardDetailsService : ICardDetailsService {
    private const string ConnectionString =
        "";

    private readonly IMongoClient _client;

    public CardDetailsService() {
        _client = new MongoClient(ConnectionString);
    }

    public async Task<IList<CardDetailsDTO>> CreateCardDetailsDTOs(IEnumerable<CardDetails> newCardDetails) {
        var db = _client.GetDatabase("MTGMetadata");
        IList<CardDetailsDTO> newCardDetailsDTOs = newCardDetails.Select(cardDetails => new CardDetailsDTO {
            Id = ObjectId.GenerateNewId().ToString(),
            ProductId = cardDetails.ProductId,
            Name = cardDetails.Name,
            CleanName = cardDetails.CleanName,
            GroupId = cardDetails.GroupId,
            SKUs = cardDetails.SKUs
        }).ToList();
        await db.GetCollection<CardDetailsDTO>("CardDetails").InsertManyAsync(newCardDetailsDTOs);
        return newCardDetailsDTOs;
    }

    public async Task<CardDetailsDTO> CreateCardDetailsDTO(CardDetails cardDetails) {
        var db = _client.GetDatabase("MTGMetadata");
        var newCardDetailsDTO = new CardDetailsDTO {
            Id = ObjectId.GenerateNewId().ToString(),
            ProductId = cardDetails.ProductId,
            Name = cardDetails.Name,
            CleanName = cardDetails.CleanName,
            GroupId = cardDetails.GroupId,
            SKUs = cardDetails.SKUs
        };
        await db.GetCollection<CardDetailsDTO>("CardDetails").InsertOneAsync(newCardDetailsDTO);
        return newCardDetailsDTO;
    }

    public async Task<CardDetailsDTO> UpdateCardDetails(CardDetails cardDetails) {
        var db = _client.GetDatabase("MTGMetadata");
        var cardFilter = new FilterDefinitionBuilder<CardDetailsDTO>().Where(cardDetailsDto =>
            cardDetailsDto.ProductId == cardDetails.ProductId);
        var updates = new UpdateDefinitionBuilder<CardDetailsDTO>()
            .Set(cardDetailsDto => cardDetailsDto.ImageUrl, cardDetails.ImageUrl)
            .Set(cardDetailsDto => cardDetailsDto.ModifiedOn, cardDetails.ModifiedOn)
            .Set(cardDetailsDto => cardDetailsDto.SKUs, cardDetails.SKUs)
            .Set(cardDetailsDto => cardDetailsDto.ExtendedData, cardDetails.ExtendedData);

        var updatedCardDetails =
            await db.GetCollection<CardDetailsDTO>("CardDetails").FindOneAndUpdateAsync(cardFilter, updates);
        return updatedCardDetails;
    }

    public IList<CardDetailsDTO> GetCardDetailsByName(string cardName) {
        var db = _client.GetDatabase("MTGMetadata");
        var cardMatch = db.GetCollection<CardDetailsDTO>("CardDetails").AsQueryable()
            .Where(card => card.Name.ToLower() == cardName.ToLower());
        return cardMatch.ToList();
    }

    public CardSku? GetSkuByCardNameAndSetId(
        string cardName, 
        int setId, 
        Condition condition = Condition.Nm,
        Printing printing = Printing.Normal, 
        Language language = Language.EN) {
        
        var db = _client.GetDatabase("MTGMetadata");
        CardDetailsDTO? matchingCardDetails = db.GetCollection<CardDetailsDTO>("CardDetails").AsQueryable()
            .FirstOrDefault(
                card => card.Name.ToLower() == cardName.ToLower() && card.GroupId == setId);
        if (matchingCardDetails == null) return null;
        
        matchingCardDetails.SKUs ??= new List<CardSku>();
        var matchingSku = matchingCardDetails.SKUs.FirstOrDefault(
            sku => sku.ConditionId == (int) condition && sku.PrintingId == (int) printing && sku.LanguageId == (int) language);
        return matchingSku;
    }

    public CardSku? GetSkuByCardId(
        int cardId, 
        Condition condition = Condition.Nm, 
        Printing printing = Printing.Normal, 
        Language language = Language.EN) {
        
        var db = _client.GetDatabase("MTGMetadata");
        CardDetailsDTO? matchingCardDetails = db.GetCollection<CardDetailsDTO>("CardDetails").AsQueryable()
            .FirstOrDefault(
                card => card.ProductId == cardId);
        if (matchingCardDetails == null) return null;

        matchingCardDetails.SKUs ??= new List<CardSku>();
        var matchingSku = matchingCardDetails.SKUs.FirstOrDefault(sku =>
            sku.ConditionId == (int) condition && sku.PrintingId == (int) printing && sku.LanguageId == (int) language);
        return matchingSku;
    }
}