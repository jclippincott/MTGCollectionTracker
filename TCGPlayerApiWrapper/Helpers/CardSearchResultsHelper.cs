using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DatabaseModels;

namespace TCGPlayerApiWrapper.Helpers;

public static class CardSearchResultsHelper {
    public static IList<CardSearchResult> TransformCardDetailsToCardSearchResult(IEnumerable<CardDetailsDTO> cardDetails, IList<SetDTO> matchingSets) {
        return cardDetails.Select(card => new CardSearchResult() {
            GroupId = card.GroupId,
            ProductId = card.ProductId,
            CardImageUrl = card.ImageUrl,
            SetSymbolImageUrl = matchingSets.Single(set => set.GroupId == card.GroupId).SetSymbolImageUrl
        }).ToList();
    }
}