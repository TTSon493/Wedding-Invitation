using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface ICompanyRepository : IRepository<Company>
{
    void Update(Company company);
    void UpdateRange(IEnumerable<Company> companies);
}