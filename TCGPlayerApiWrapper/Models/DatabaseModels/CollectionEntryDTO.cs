using MongoDB.Bson.Serialization.Attributes;

namespace TCGPlayerApiWrapper.Models.DatabaseModels;

public class CollectionEntryDTO {
    [BsonId]
    // Read only
    public string Id { get; set; }
    public int Quantity { get; set; }
    public int SkuId { get; set; }
    public decimal UsdValue { get; set; }
    public DateTime LastValueRetrievalTime { get; set; }
    public DateTime LastModifiedDate { get; set; }
}