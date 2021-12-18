using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TCGPlayerApiWrapper.Models.DTOs; 

public class MtgCardDetailsDTO {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public int ProductId { get; set; }
    
    public string Name { get; set; }
    
    public string CleanName { get; set; }
    
    public int GroupId { get; set; }
    
    [BsonRepresentation(BsonType.Array)]
    public IList<MtgCardSKU> SKUs { get; set; }
}