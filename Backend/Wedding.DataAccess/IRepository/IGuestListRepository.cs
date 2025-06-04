using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IGuestListRepository : IRepository<GuestList>
{
    void Update(GuestList guestList);
    void UpdateRange(IEnumerable<GuestList> guestLists);
    Task<GuestList?> GetById(Guid id);
    Task<GuestList> AddAsync(GuestList guestList);
}