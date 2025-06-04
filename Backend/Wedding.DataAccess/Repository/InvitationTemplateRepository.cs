using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace Wedding.DataAccess.Repository;

public class InvitationTemplateRepository : Repository<InvitationTemplate>, IInvitationTemplateRepository
{
    private readonly ApplicationDbContext _context;

    public InvitationTemplateRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(InvitationTemplate invitationTemplate)
    {
        _context.InvitationTemplates.Update(invitationTemplate);
    }
    
    public void UpdateRange(IEnumerable<InvitationTemplate> invitationTemplates)
    {
        _context.InvitationTemplates.UpdateRange(invitationTemplates);
    }
    
    public async Task<InvitationTemplate?> GetById(Guid id)
    {
        return await _context.InvitationTemplates.FirstOrDefaultAsync(x => x.TemplateId == id);

    }
    
    public async Task<InvitationTemplate> AddAsync(InvitationTemplate invitationTemplate)
    {
        var entityEntry = await _context.InvitationTemplates.AddAsync(invitationTemplate);
        return entityEntry.Entity;
    }
}