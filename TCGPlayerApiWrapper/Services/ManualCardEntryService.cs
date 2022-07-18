using MongoDB.Bson;
using TCGPlayerApiWrapper.ApiAccess;
using TCGPlayerApiWrapper.DBAccess;
using TCGPlayerApiWrapper.Enums;
using TCGPlayerApiWrapper.Helpers;
using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DatabaseModels;
using static System.Enum;

namespace TCGPlayerApiWrapper.Services; 

public class ManualCardEntryService {
    public static async void StartCardEntryPrompt() {
        var authTokenRetriever = new AuthApiWrapper();
        var baseUrl = new Uri("https://api.tcgplayer.com");
        var accessToken = authTokenRetriever.AuthToken;
        if (accessToken == null) {
            throw new Exception("Couldn't retrieve auth token");
        }

        var cardDetailsService = new CardDetailsService();
        var collectionEntryService = new CollectionEntryService();
        var setService = new SetService();
        var pricingApiWrapper = new PricingApiWrapper(baseUrl, accessToken);

        var addAnotherCard = true;

        while (addAnotherCard) {
            Console.WriteLine("Please enter the name of the card you'd like to add");
            Console.WriteLine();
            string? cardName = Console.ReadLine();
            if (cardName == null) {
                Console.WriteLine("You must enter a valid card name");
                Environment.Exit(0);
            }

            IList<CardDetailsDTO> matchingCardDetails = cardDetailsService.GetCardDetailsByName(cardName);
            IList<SetDTO> matchingSets =
                setService.GetSetsByIds(matchingCardDetails.Select(cardDetails => cardDetails.GroupId).ToList());
            Console.WriteLine();
            Console.WriteLine(
                $"Found {matchingSets.Count} matching versions. Please select the set your card is from: ");
            Console.WriteLine();
            for (var i = 0; i < matchingSets.Count; i++) {
                Console.WriteLine($"{i}: {matchingSets[i].Name}");
            }

            Console.WriteLine();

            var selectedCard = new CardDetailsDTO();
            try {
                var setIndex = Convert.ToInt32(Console.ReadLine());
                if (setIndex > matchingSets.Count - 1) {
                    Console.WriteLine("You must choose one of the provided options");
                    Environment.Exit(0);
                }

                int setId = matchingSets[setIndex].GroupId;
                selectedCard = matchingCardDetails.FirstOrDefault(cardDetails => cardDetails.GroupId == setId);
            }
            catch (FormatException) {
                Console.WriteLine("You must enter a valid number");
                Environment.Exit(0);
            }
            Console.WriteLine();


            if (selectedCard == null) {
                Console.WriteLine("Couldn't find a card matching the provided parameters");
                Environment.Exit(0);
            }

            Console.WriteLine("Please enter the number corresponding to the condition");
            foreach (int i in GetValues(typeof(Condition))) {
                Console.WriteLine($"{i}: {((Condition)i).GetDescription()}");
            }
            Console.WriteLine();


            bool conditionParseSucceeded = TryParse(Console.ReadLine(), out Condition condition);
            if (!conditionParseSucceeded) {
                Console.WriteLine("Couldn't interpret response, setting condition to NM");
                condition = Condition.Nm;
            }

            Console.WriteLine();

            var printing = Printing.Normal;
            Console.WriteLine("Foil? (y/n)");
            Console.WriteLine();
            string? isFoil = Console.ReadLine();
            if (isFoil != null && isFoil.ToLower() == "y") {
                printing = Printing.Foil;
            }

            var language = Language.EN;
            Console.WriteLine("Is the card in English? (y/n)");
            Console.WriteLine();
            string? isEnglish = Console.ReadLine();
            if (isEnglish != null && isEnglish.ToLower() == "n") {
                Console.WriteLine("Please enter the number corresponding to the language");
                foreach (int i in GetValues(typeof(Language))) {
                    if (i > 1) {
                        Console.WriteLine($"{i}: {((Language) i).GetDescription()}");
                    }
                }

                bool languageParseSucceeded = TryParse(Console.ReadLine(), out language);
                if (!languageParseSucceeded) {
                    Console.WriteLine("Couldn't interpret response, setting language back to English");
                    language = Language.EN;
                }

                Console.WriteLine();
            }

            CardSku? matchingSku = cardDetailsService.GetSkuByCardId(selectedCard.ProductId, condition, printing, language);
            if (matchingSku == null) {
                Console.WriteLine("Couldn't find a SKU for the details provided");
                Environment.Exit(0);
            }
            
            
            int quantityAlreadyInCollection =
                await collectionEntryService.QuantityOfSkuInCollection(matchingSku.SkuId);
            if (quantityAlreadyInCollection != -1) {
                Console.WriteLine($"You already have {quantityAlreadyInCollection} of this card in the collection");
                Console.WriteLine("What would you like to update the quantity to?");
            }
            else {
                Console.WriteLine("Please enter the quantity:");
            }


            var cardQuantity = 0;
            try {
                cardQuantity = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException) {
                Console.WriteLine("You must enter a valid number");
                Environment.Exit(0);
            }

            Console.WriteLine();

            var collectionEntry = new CollectionEntryDTO {
                Id = ObjectId.GenerateNewId().ToString(),
                Quantity = cardQuantity,
                SkuId = matchingSku.SkuId,
                UsdValue = 0,
                LastModifiedDate = DateTime.Now,
                LastValueRetrievalTime = DateTime.Now
            };
            
            pricingApiWrapper.GetPriceForCollectionEntry(collectionEntry);
            Console.WriteLine($"{(quantityAlreadyInCollection > 0 ? "Updating" : "Adding")} entry in collection");
            bool upsertResult = await collectionEntryService.UpsertCollectionEntry(collectionEntry);
            
            Console.WriteLine(upsertResult
                ? $"Successfully {(quantityAlreadyInCollection > 0 ? "updated" : "added")} card"
                : "Failed to add card");

            Console.WriteLine($"Currently have {collectionEntry.Quantity} copies worth ${collectionEntry.UsdValue} each ({collectionEntry.Quantity * collectionEntry.UsdValue} total)");
            
            Console.WriteLine();
            Console.WriteLine("Would you like to add/update another entry? (y/n)");
            string? addAnotherResponse = Console.ReadLine();
            if (addAnotherResponse == null || addAnotherResponse.ToLower() != "y") {
                addAnotherCard = false;
            }
        }
    }
}