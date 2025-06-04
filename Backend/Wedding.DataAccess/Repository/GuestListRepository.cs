using Microsoft.EntityFrameworkCore;
using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class GuestListRepository : Repository<GuestList>, IGuestListRepository
{
    private readonly ApplicationDbContext _context;

    public GuestListRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(GuestList guestList)
    {
        _context.GuestLists.Update(guestList);
    }

    public void UpdateRange(IEnumerable<GuestList> guestLists)
    {
        _context.GuestLists.UpdateRange(guestLists);
    }

    public async Task<GuestList?> GetById(Guid id)
    {
        return await _context.GuestLists.FirstOrDefaultAsync(x => x.GuestListId == id);

    }

    public async Task<GuestList> AddAsync(GuestList guestList)
    {
        var entityEntry = await _context.GuestLists.AddAsync(guestList);
        return entityEntry.Entity;
    }
}