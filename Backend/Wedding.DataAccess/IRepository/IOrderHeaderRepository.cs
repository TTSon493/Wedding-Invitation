using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
    void Update(OrderHeader orderHeader);
    void UpdateRange(IEnumerable<OrderHeader> orderHeaders);
}