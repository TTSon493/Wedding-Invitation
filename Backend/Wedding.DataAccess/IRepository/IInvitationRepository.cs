using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IInvitationRepository : IRepository<Invitation>
{
    void Update(Invitation invitation);
    void UpdateRange(IEnumerable<Invitation> invitations);
    Task<Invitation?> GetById(Guid id);
    Task<Invitation> AddAsync(Invitation invitation);
}