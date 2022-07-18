namespace TCGPlayerApiWrapper.Models; 

public class CardPrice {
    // Synonymous with Sku
    public int ProductConditionId { get; set; }
    public decimal Price { get; set; }
    public decimal LowestRange { get; set; }
    public decimal HighestRange { get; set; }
}