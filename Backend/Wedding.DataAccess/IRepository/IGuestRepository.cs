using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IGuestRepository : IRepository<Guest>
{
    void Update(Guest guest);
    void UpdateRange(IEnumerable<Guest> guests);
    Task<Guest?> GetById(Guid id);
    Task<Guest> AddAsync(Guest guest);
}