using Microsoft.EntityFrameworkCore;
using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class GuestRepository : Repository<Guest>, IGuestRepository
{
    private readonly ApplicationDbContext _context;

    public GuestRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Guest guest)
    {
        _context.Guests.Update(guest);
    }

    public void UpdateRange(IEnumerable<Guest> guests)
    {
        _context.Guests.UpdateRange(guests);
    }

    public async Task<Guest?> GetById(Guid id)
    {
        return await _context.Guests.FirstOrDefaultAsync(x => x.GuestId == id);

    }

    public async Task<Guest> AddAsync(Guest guest)
    {
        var entityEntry = await _context.Guests.AddAsync(guest);
        return entityEntry.Entity;
    }
}