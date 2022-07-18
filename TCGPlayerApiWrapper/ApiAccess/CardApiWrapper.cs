using RestSharp;
using TCGPlayerApiWrapper.Enums;
using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.ApiResponse;

namespace TCGPlayerApiWrapper.ApiAccess; 

public class CardApiWrapper {
    private Uri _baseUrl;
    private AuthToken _accessToken;
    
    public CardApiWrapper(Uri baseUrl, AuthToken accessToken) {
        _baseUrl = baseUrl;
        _accessToken = accessToken;
    }

    public IList<CardDetails> GetAllCards() {
        IRestClient client = new RestClient(_baseUrl);
        IRestRequest request = new RestRequest($"/catalog/products", Method.GET);
        request.AddHeader("Authorization", $"Bearer {_accessToken.AccessToken}");
        request.AddOrUpdateParameter("categoryId", (int)Product.Mtg);
        request.AddOrUpdateParameter("includeSkus", true);

        int limit = 1;
        int offset = 0;
        List<CardDetails> itemsRetrieved = new List<CardDetails>();

        request.AddOrUpdateParameter("limit", limit);
        request.AddOrUpdateParameter("offset", offset);
        IRestResponse<BulkApiResponse<CardDetails>> response = client.Execute<BulkApiResponse<CardDetails>>(request);
        
        itemsRetrieved.AddRange(response.Data.Results);
        limit = 100;
        offset = 1;
        while (itemsRetrieved.Count < response.Data.TotalItems) {
            request.AddOrUpdateParameter("limit", limit);
            request.AddOrUpdateParameter("offset", offset);
        
            response = client.Execute<BulkApiResponse<CardDetails>>(request);
            itemsRetrieved.AddRange(response.Data.Results);
            offset += 100;
        }

        return itemsRetrieved;
    }
    
}