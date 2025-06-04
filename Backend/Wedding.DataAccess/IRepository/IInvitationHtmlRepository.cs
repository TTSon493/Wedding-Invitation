using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IInvitationHtmlRepository : IRepository<InvitationHtml>
{
    void Update(InvitationHtml invitationHtml);
    void UpdateRange(IEnumerable<InvitationHtml> invitationHtmls);
    Task<InvitationHtml?> GetById(Guid id);
    Task<InvitationHtml> AddAsync(InvitationHtml invitationHtml);
}