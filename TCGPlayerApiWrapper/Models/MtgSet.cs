namespace TCGPlayerApiWrapper.Models; 

public class MtgSet {
    public int GroupId { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public bool IsSupplemental { get; set; }
    public string PublishedOn { get; set; }
    public string ModifiedOn { get; set; }
}