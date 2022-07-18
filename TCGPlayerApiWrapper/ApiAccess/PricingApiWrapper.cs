using System.Collections;
using RestSharp;
using TCGPlayerApiWrapper.Enums;
using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.ApiResponse;
using TCGPlayerApiWrapper.Models.DatabaseModels;

namespace TCGPlayerApiWrapper.ApiAccess; 

public class PricingApiWrapper {
    private Uri _baseUrl;
    private AuthToken _accessToken;
    
    public PricingApiWrapper(Uri baseUrl, AuthToken accessToken) {
        _baseUrl = baseUrl;
        _accessToken = accessToken;
    }

    public IList<CollectionEntryDTO> GetPricesForCollectionEntries(IList<CollectionEntryDTO> collectionEntries) {
        IList<CollectionEntryDTO> updatedCollectionEntries = new List<CollectionEntryDTO>();
        IRestClient client = new RestClient(_baseUrl);
        foreach (var collectionEntry in collectionEntries) {
            IRestRequest request = new RestRequest($"/pricing/marketprices/{collectionEntry.SkuId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {_accessToken.AccessToken}");
            IRestResponse<ApiResponse<CardPrice>> response = client.Execute<ApiResponse<CardPrice>>(request);
            if (response.IsSuccessful) {
                collectionEntry.UsdValue = response.Data.Results[0].Price;
                collectionEntry.LastValueRetrievalTime = DateTime.Now;
            }

            updatedCollectionEntries.Add(collectionEntry);
        }
        return updatedCollectionEntries;
    }

    public CollectionEntryDTO GetPriceForCollectionEntry(CollectionEntryDTO collectionEntry) {
        IRestClient client = new RestClient(_baseUrl);
        IRestRequest request = new RestRequest($"/pricing/marketprices/{collectionEntry.SkuId}", Method.GET);
        request.AddHeader("Authorization", $"Bearer {_accessToken.AccessToken}");
        IRestResponse<ApiResponse<CardPrice>> response = client.Execute<ApiResponse<CardPrice>>(request);
       
        if (response.IsSuccessful) {
            collectionEntry.UsdValue = response.Data.Results[0].Price;
            collectionEntry.LastValueRetrievalTime = DateTime.Now;
        }
        
        return collectionEntry;
    }
    
}