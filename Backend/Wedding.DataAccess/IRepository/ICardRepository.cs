using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository
{
    public interface ICardRepository : IRepository<Card>
    {
        void Update(Card card);
        void UpdateRange(IEnumerable<Card> cards);
        Task<Card?> GetById(Guid cardId);
    }
}
