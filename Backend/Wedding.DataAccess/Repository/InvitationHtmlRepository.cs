using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace Wedding.DataAccess.Repository;

public class InvitationHtmlRepository : Repository<InvitationHtml>, IInvitationHtmlRepository
{
    private readonly ApplicationDbContext _context;

    public InvitationHtmlRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(InvitationHtml invitationHtml)
    {
        _context.InvitationHtmls.Update(invitationHtml);
    }
    
    public void UpdateRange(IEnumerable<InvitationHtml> invitationHtmls)
    {
        _context.InvitationHtmls.UpdateRange(invitationHtmls);
    }

    public async Task<InvitationHtml?> GetById(Guid id)
    {
        return await _context.InvitationHtmls.FirstOrDefaultAsync(x => x.HtmlId == id);

    }

    public async Task<InvitationHtml> AddAsync(InvitationHtml invitationHtml)
    {
        var entityEntry = await _context.InvitationHtmls.AddAsync(invitationHtml);
        return entityEntry.Entity;
    }
}