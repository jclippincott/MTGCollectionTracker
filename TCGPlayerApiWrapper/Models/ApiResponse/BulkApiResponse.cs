namespace TCGPlayerApiWrapper.Models.ApiResponse; 

public class BulkApiResponse<T>: ApiResponse<T> {
    public int TotalItems { get; set; }
}