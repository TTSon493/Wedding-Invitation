using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace Wedding.DataAccess.Repository;

public class InvitationRepository : Repository<Invitation>, IInvitationRepository
{
    private readonly ApplicationDbContext _context;

    public InvitationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Invitation invitation)
    {
        _context.Invitations.Update(invitation);
    }
    
    public void UpdateRange(IEnumerable<Invitation> invitations)
    {
        _context.Invitations.UpdateRange(invitations);
    }
    
    public async Task<Invitation?> GetById(Guid id)
    {
        return await _context.Invitations.FirstOrDefaultAsync(x => x.InvitationId == id);

    }
    
    public async Task<Invitation> AddAsync(Invitation invitation)
    {
        var entityEntry = await _context.Invitations.AddAsync(invitation);
        return entityEntry.Entity;
    }
}