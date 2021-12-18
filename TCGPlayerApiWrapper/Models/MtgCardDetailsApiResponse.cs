namespace TCGPlayerApiWrapper.Models;

public class MtgCardDetailsApiResponse {
    public int TotalItems { get; set; }
    public bool Success { get; set; }
    public List<string> Errors { get; set; }
    public List<MtgCardDetails> Results { get; set; }
}