using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IEventRepository : IRepository<Event>
{
    void Update(Event events);
    void UpdateRange(IEnumerable<Event> events);
    Task<Event?> GetById(Guid id);
    Task<Event> AddAsync(Event events);
}