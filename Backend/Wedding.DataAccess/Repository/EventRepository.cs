using Microsoft.EntityFrameworkCore;
using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class EventRepository : Repository<Event>, IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Event events)
    {
        _context.Events.Update(events);
    }

    public void UpdateRange(IEnumerable<Event> events)
    {
        _context.Events.UpdateRange(events);
    }

    public async Task<Event?> GetById(Guid id)
    {
        return await _context.Events.FirstOrDefaultAsync(x => x.EventId == id);

    }

    public async Task<Event> AddAsync(Event events)
    {
        var entityEntry = await _context.Events.AddAsync(events);
        return entityEntry.Entity;
    }
}