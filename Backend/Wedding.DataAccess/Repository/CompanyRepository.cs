using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Company company)
    {
        _context.Companies.Update(company);
    }

    public void UpdateRange(IEnumerable<Company> companies)
    {
        _context.Companies.UpdateRange(companies);
    }
}