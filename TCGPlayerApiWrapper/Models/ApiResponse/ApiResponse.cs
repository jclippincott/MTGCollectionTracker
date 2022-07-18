namespace TCGPlayerApiWrapper.Models.ApiResponse; 

public class ApiResponse<T> {
    public bool Success { get; set; }
    public List<string> Errors { get; set; }
    public List<T> Results { get; set; }
}