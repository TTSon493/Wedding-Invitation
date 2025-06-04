using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IBalanceRepository : IRepository<Balance>
{
    void Update(Balance balance);
    void UpdateRange(IEnumerable<Balance> balances);
}