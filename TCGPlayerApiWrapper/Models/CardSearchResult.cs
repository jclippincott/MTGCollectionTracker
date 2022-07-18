namespace TCGPlayerApiWrapper.Models; 

public class CardSearchResult {
    public int ProductId { get; set; }
    public int GroupId { get; set; }
    public string? CardImageUrl { get; set; }
    public string? SetSymbolImageUrl { get; set; }
}