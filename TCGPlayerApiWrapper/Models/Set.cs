namespace TCGPlayerApiWrapper.Models; 

public class Set {
    public int GroupId { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public bool IsSupplemental { get; set; }
    public string PublishedOn { get; set; }
    public string ModifiedOn { get; set; }
    public string? SetSymbolImageUrl { get; set; }

}