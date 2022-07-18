using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DatabaseModels;

namespace TCGPlayerApiWrapper.DBAccess.Interface;

public interface ICardDetailsService : IDatabaseAccessService {
    /// <summary>
    ///     Creates a list of CardDetailsDTOs from a list of CardDetails
    ///     Currently, this just adds a BSONId to each of them
    /// </summary>
    /// <param name="newCardDetails"></param>
    /// <returns></returns>
    public Task<IList<CardDetailsDTO>> CreateCardDetailsDTOs(IEnumerable<CardDetails> newCardDetails);

    /// <summary>
    ///     Creates a CardDetailsDTO from a CardDetails object
    ///     Currently, this just adds a BSONId to it
    /// </summary>
    /// <param name="newCardDetails"></param>
    /// <returns></returns>
    public Task<CardDetailsDTO> CreateCardDetailsDTO(CardDetails newCardDetails);

    /// <summary>
    ///     Updates a cardDetails object with the values provided in the parameter
    /// </summary>
    /// <param name="cardDetails"></param>
    /// <returns></returns>
    public Task<CardDetailsDTO> UpdateCardDetails(CardDetails cardDetails);

    /// <summary>
    ///     Returns a list of all CardDetailsDTOs that match the provided name (case-insensitive)
    /// </summary>
    /// <param name="cardName"></param>
    /// <returns></returns>
    public IList<CardDetailsDTO> GetCardDetailsByName(string cardName);
}