using TCGPlayerApiWrapper.ApiAccess;
using TCGPlayerApiWrapper.DBAccess;
using TCGPlayerApiWrapper.Helpers;
using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DatabaseModels;

namespace TCGPlayerApiWrapper.Services; 

public class CardSearchResultService {
    public static async Task GetSearchResultsByCardName(Uri baseUrl, AuthToken accessToken) {
        var setApiWrapper = new SetApiWrapper(baseUrl, accessToken);

        var cardDetailsService = new CardDetailsService();
        var setService = new SetService();

        IList<CardDetailsDTO> fullCardDetails = cardDetailsService.GetCardDetailsByName("Armageddon");
        IList<int> setIds = fullCardDetails.Select(card => card.GroupId).ToList();

        IList<SetDTO> matchingSets = setService.GetSetsByIds(setIds);

        List<SetDTO> setsMissingMedia = matchingSets.Where(set => string.IsNullOrEmpty(set.SetSymbolImageUrl)).ToList();
        foreach (var set in setsMissingMedia) {
            IList<Media> setMedia = setApiWrapper.GetSetMediaById(set.GroupId);
            if (setMedia.Count <= 0 || setMedia[0].ContentList.Count <= 0) continue;

            set.SetSymbolImageUrl = setMedia[0].ContentList.Single(content => content.DisplayOrder == 1).Url;
            await setService.UpdateSet(set);
        }

        IList<CardSearchResult> searchResults = CardSearchResultsHelper.TransformCardDetailsToCardSearchResult(fullCardDetails, matchingSets);
    }
}