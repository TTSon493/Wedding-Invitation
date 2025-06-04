using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IPrivacyRepository : IRepository<Privacy>
{
    void Update(Privacy privacy);
    void UpdateRange(IEnumerable<Privacy> privacies);
}