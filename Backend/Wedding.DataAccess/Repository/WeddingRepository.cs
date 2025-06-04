using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace Wedding.DataAccess.Repository;

public class WeddingRepository : Repository<Model.Domain.Wedding>, IWeddingRepository
{
    private readonly ApplicationDbContext _context;

    public WeddingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Model.Domain.Wedding wedding)
    {
        _context.Weddings.Update(wedding);
    }
    
    public void UpdateRange(IEnumerable<Model.Domain.Wedding> weddings)
    {
        _context.Weddings.UpdateRange(weddings);
    }
    
    public async Task<Model.Domain.Wedding?> GetById(Guid id)
    {
        return await _context.Weddings.FirstOrDefaultAsync(x => x.WeddingId == id);

    }
    
    public async Task<Model.Domain.Wedding> AddAsync(Model.Domain.Wedding wedding)
    {
        var entityEntry = await _context.Weddings.AddAsync(wedding);
        return entityEntry.Entity;
    }
}