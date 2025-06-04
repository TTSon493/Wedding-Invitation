using Microsoft.EntityFrameworkCore;
using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class EventPhotoRepository : Repository<EventPhoto>, IEventPhotoRepository
{
    private readonly ApplicationDbContext _context;

    public EventPhotoRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(EventPhoto eventPhoto)
    {
        _context.EventPhotos.Update(eventPhoto);
    }

    public void UpdateRange(IEnumerable<EventPhoto> eventPhoto)
    {
        _context.EventPhotos.UpdateRange(eventPhoto);
    }

    public async Task<EventPhoto?> GetById(Guid id)
    {
        return await _context.EventPhotos.FirstOrDefaultAsync(x => x.EventPhotoId == id);

    }

    public async Task<EventPhoto> AddAsync(EventPhoto eventPhoto)
    {
        var entityEntry = await _context.EventPhotos.AddAsync(eventPhoto);
        return entityEntry.Entity;
    }
}