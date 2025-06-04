using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class BalanceRepository : Repository<Balance>, IBalanceRepository
{
    private readonly ApplicationDbContext _context;

    public BalanceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Balance balance)
    {
        _context.Balances.Update(balance);
    }

    public void UpdateRange(IEnumerable<Balance> balances)
    {
        _context.Balances.UpdateRange(balances);
    }
}