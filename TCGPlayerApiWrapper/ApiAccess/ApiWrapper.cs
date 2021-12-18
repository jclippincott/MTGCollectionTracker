using RestSharp;
using TCGPlayerApiWrapper.Enums;
using TCGPlayerApiWrapper.Models;

namespace TCGPlayerApiWrapper.ApiAccess; 

public class ApiWrapper {
    private Uri _baseUrl;
    private AuthToken _accessToken;
    
    public ApiWrapper(Uri baseUrl, AuthToken accessToken) {
        _baseUrl = baseUrl;
        _accessToken = accessToken;
    }

    public IList<MtgSet> GetAllSets() {
        IRestClient client = new RestClient(_baseUrl);
        IRestRequest request = new RestRequest($"/catalog/categories/{(int)Product.Mtg}/groups", Method.GET);
        request.AddHeader("Authorization", $"Bearer {_accessToken.AccessToken}");

        int limit = 1;
        int offset = 0;
        List<MtgSet> itemsRetrieved = new List<MtgSet>();

        request.AddOrUpdateParameter("limit", limit);
        request.AddOrUpdateParameter("offset", offset);
        IRestResponse<MtgSetApiResponse> response = client.Execute<MtgSetApiResponse>(request);
        itemsRetrieved.AddRange(response.Data.Results);
        limit = 100;
        offset = 1;
        while (itemsRetrieved.Count < response.Data.TotalItems) {
            request.AddOrUpdateParameter("limit", limit);
            request.AddOrUpdateParameter("offset", offset);

            response = client.Execute<MtgSetApiResponse>(request);
            itemsRetrieved.AddRange(response.Data.Results);
            offset += 100;
        }
        
        return itemsRetrieved;
    }

    public IList<MtgCardDetails> GetAllCards() {
        IRestClient client = new RestClient(_baseUrl);
        IRestRequest request = new RestRequest($"/catalog/products", Method.GET);
        request.AddHeader("Authorization", $"Bearer {_accessToken.AccessToken}");
        request.AddOrUpdateParameter("categoryId", Product.Mtg);
        request.AddOrUpdateParameter("includeSkus", true);

        int limit = 1;
        int offset = 0;
        List<MtgCardDetails> itemsRetrieved = new List<MtgCardDetails>();

        request.AddOrUpdateParameter("limit", limit);
        request.AddOrUpdateParameter("offset", offset);
        IRestResponse<MtgCardDetailsApiResponse> response = client.Execute<MtgCardDetailsApiResponse>(request);
        
        itemsRetrieved.AddRange(response.Data.Results);
        limit = 100;
        offset = 1;
        while (itemsRetrieved.Count < response.Data.TotalItems) {
            request.AddOrUpdateParameter("limit", limit);
            request.AddOrUpdateParameter("offset", offset);
        
            response = client.Execute<MtgCardDetailsApiResponse>(request);
            itemsRetrieved.AddRange(response.Data.Results);
            offset += 100;
        }

        return itemsRetrieved;
    }
}