namespace TCGPlayerApiWrapper.Models; 

public class MtgCardDetails {
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string CleanName { get; set; }
    public int GroupId { get; set; }
    public IList<MtgCardSKU> SKUs { get; set; }
}