namespace TCGPlayerApiWrapper.Models; 

public class MtgSetApiResponse {
    public int TotalItems { get; set; }
    public bool Success { get; set; }
    public List<string> Errors { get; set; }
    public List<MtgSet> Results { get; set; }
}