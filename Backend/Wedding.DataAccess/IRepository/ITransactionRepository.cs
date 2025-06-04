using Wedding.Model.Domain;
using Wedding.DataAccess.IRepository;

namespace Wedding.DataAccess.IRepository;

public interface ITransactionRepository : IRepository<Transaction>
{
    void Update(Transaction transaction);
    void UpdateRange(IEnumerable<Transaction> transactions);
}