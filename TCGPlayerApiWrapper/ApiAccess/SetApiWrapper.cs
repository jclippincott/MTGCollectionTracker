using RestSharp;
using TCGPlayerApiWrapper.Enums;
using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.ApiResponse;

namespace TCGPlayerApiWrapper.ApiAccess;

public class SetApiWrapper {
    private Uri _baseUrl;
    private AuthToken _accessToken;

    public SetApiWrapper(Uri baseUrl, AuthToken accessToken) {
        _baseUrl = baseUrl;
        _accessToken = accessToken;
    }

    public IList<Set> GetAllSets() {
        IRestClient client = new RestClient(_baseUrl);
        IRestRequest request = new RestRequest($"/catalog/categories/{(int)Product.Mtg}/groups", Method.GET);
        request.AddHeader("Authorization", $"Bearer {_accessToken.AccessToken}");

        int limit = 1;
        int offset = 0;
        List<Set> itemsRetrieved = new List<Set>();

        request.AddOrUpdateParameter("limit", limit);
        request.AddOrUpdateParameter("offset", offset);
        IRestResponse<BulkApiResponse<Set>> response = client.Execute<BulkApiResponse<Set>>(request);
        itemsRetrieved.AddRange(response.Data.Results);
        limit = 100;
        offset = 1;
        while (itemsRetrieved.Count < response.Data.TotalItems) {
            request.AddOrUpdateParameter("limit", limit);
            request.AddOrUpdateParameter("offset", offset);

            response = client.Execute<BulkApiResponse<Set>>(request);
            itemsRetrieved.AddRange(response.Data.Results);
            offset += 100;
        }

        return itemsRetrieved;
    }

    public IList<Media> GetSetMediaById(int setId) {
        IRestClient client = new RestClient(_baseUrl);
        IRestRequest request = new RestRequest($"/catalog/groups/{setId}/media", Method.GET);
        request.AddHeader("Authorization", $"Bearer {_accessToken.AccessToken}");
        
        var response = client.Execute<ApiResponse<Media>>(request);
        return response.Data.Results;
    }
    
}