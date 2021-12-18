using Microsoft.Extensions.Configuration;
using TCGPlayerApiWrapper.ApiAccess;
using TCGPlayerApiWrapper.DBAccess;
using TCGPlayerApiWrapper.Models;

namespace TCGPlayerApiWrapper {
    public static class Program {
        
        public static async Task Main() {
            AuthTokenRetriever authTokenRetriever = new AuthTokenRetriever();
            Uri baseUrl = new Uri("https://api.tcgplayer.com");
            AuthToken? accessToken = authTokenRetriever.AuthToken;
            if (accessToken == null) {
                throw new Exception("Couldn't retrieve auth token");
            }

            ApiWrapper apiWrapper = new ApiWrapper(baseUrl, accessToken);
            IList<MtgCardDetails> mtgCardDetails = apiWrapper.GetAllCards();
            MtgCardDetailsService cardDetailsService = new MtgCardDetailsService();
            var newCardDetails = await cardDetailsService.CreateMtgCardDetailsDTOs(mtgCardDetails);
            
        }
        
    }
 
}