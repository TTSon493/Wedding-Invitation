using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface ITermOfUseRepository : IRepository<TermOfUse>
{
    void Update(TermOfUse termOfUse);
    void UpdateRange(IEnumerable<TermOfUse> termOfUses);
}