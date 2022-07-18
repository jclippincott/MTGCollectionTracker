using TCGPlayerApiWrapper.Models;
using TCGPlayerApiWrapper.Models.DatabaseModels;

namespace TCGPlayerApiWrapper.DBAccess.Interface;

public interface ISetService : IDatabaseAccessService {
    /// <summary>
    ///     Creates a list of SetDTO from a list of Sets
    ///     Currently, this just adds a BSONId to each of them
    /// </summary>
    /// <param name="newSets"></param>
    /// <returns></returns>
    public Task<IList<SetDTO>> CreateSetDTOs(IList<Set> newSets);

    /// <summary>
    ///     Creates a SetDTO from a Set object
    ///     Currently, this just adds a BSONId to it
    ///     ///
    /// </summary>
    /// <param name="newSet"></param>
    /// <returns></returns>
    public Task<SetDTO> CreateSetDTO(Set newSet);

    /// <summary>
    ///     Returns a list of SetDTOs matching the provided setIds
    ///     setId corresponds to GroupId property on Set
    /// </summary>
    /// <param name="setIds"></param>
    /// <returns></returns>
    public IList<SetDTO> GetSetsByIds(IList<int> setIds);
}