using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace Wedding.DataAccess.Repository;

public class CardManagementRepository : Repository<CardManagement>, ICardManagementRepository
{
    private readonly ApplicationDbContext _context;

    public CardManagementRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(CardManagement cardManagement)
    {
        _context.CardManagements.Update(cardManagement);
    }
    
    public void UpdateRange(IEnumerable<CardManagement> cardManagements)
    {
        _context.CardManagements.UpdateRange(cardManagements);
    }
    
    public async Task<CardManagement?> GetById(Guid id)
    {
        return await _context.CardManagements.FirstOrDefaultAsync(x => x.CardId == id);

    }
    
    public async Task<CardManagement> AddAsync(CardManagement cardManagement)
    {
        var entityEntry = await _context.CardManagements.AddAsync(cardManagement);
        return entityEntry.Entity;
    }
}