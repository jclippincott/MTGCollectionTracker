using MongoDB.Bson.Serialization.Attributes;

namespace TCGPlayerApiWrapper.Models; 

public class CardDetails {
    [BsonId]
    // Read only
    public string Id { get; set; }
    
    // Read only
    public int ProductId { get; set; }

    // Read only
    public string Name { get; set; }

    // Read only
    public string CleanName { get; set; }

    // Read only
    public int GroupId { get; set; }

    public IList<CardSku> SKUs { get; set; }
    public string ImageUrl { get; set; }
    public ExtendedCardDetails? ExtendedData { get; set; }
    public string ModifiedOn { get; set; }
}