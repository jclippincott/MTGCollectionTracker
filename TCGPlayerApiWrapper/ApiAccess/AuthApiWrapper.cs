using RestSharp;
using TCGPlayerApiWrapper.Models;

namespace TCGPlayerApiWrapper.ApiAccess;

public class AuthApiWrapper {
    public AuthToken? AuthToken;

    public AuthApiWrapper() {
        AuthToken = InitializeAuthToken();
    }

    public AuthToken? InitializeAuthToken() {
        string CLIENT_ID = "";
        string CLIENT_SECRET = "";

        Uri baseUrl = new Uri("https://api.tcgplayer.com");
        IRestClient client = new RestClient(baseUrl);
        IRestRequest request = new RestRequest("token", Method.POST);

        request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);                          
        request.AddParameter("client_id", CLIENT_ID, ParameterType.GetOrPost);
        request.AddParameter("client_secret", CLIENT_SECRET, ParameterType.GetOrPost);
        request.AddHeader("User-Agent", "Personal-Collection-Tracker");

        try {
            IRestResponse<AuthToken> response = client.Execute<AuthToken>(request);
            if (response.IsSuccessful) {
                return response.Data;
            }

            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message, ex);
        }
    }
}