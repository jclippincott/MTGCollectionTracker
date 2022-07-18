using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TCGPlayerApiWrapper.Models.DatabaseModels; 

public class SetDTO {
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public int GroupId { get; set; }
    
    public string Name { get; set; }
    
    public string Abbreviation { get; set; }
    
    public bool IsSupplemental { get; set; }
    
    public string PublishedOn { get; set; }
    
    public string ModifiedOn { get; set; }
    
    public string? SetSymbolImageUrl { get; set; }


}