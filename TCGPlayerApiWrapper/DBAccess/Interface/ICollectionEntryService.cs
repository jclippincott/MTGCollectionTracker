using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DatabaseModels;

namespace TCGPlayerApiWrapper.DBAccess.Interface;

public interface ICollectionEntryService : IDatabaseAccessService {
    /// <summary>
    ///     Updates or inserts collection entries based on the list of CollectionEntry objects provided
    /// </summary>
    /// <param name="newCollectionEntries"></param>
    /// <returns></returns>
    public Task<IList<bool>> UpsertCollectionEntries(IList<CollectionEntryDTO> newCollectionEntries);

    /// <summary>
    ///     Updates or inserts a single collection entry from the provided CollectionEntry object
    /// </summary>
    /// <param name="newCollectionEntry"></param>
    /// <returns></returns>
    public Task<bool> UpsertCollectionEntry(CollectionEntryDTO newCollectionEntry);

    /// <summary>
    /// Returns the quantity of the given sku in the collection (if any exist)
    /// Default is -1 if none are found
    /// </summary>
    /// <returns></returns>
    public Task<int> QuantityOfSkuInCollection(int skuId);
}