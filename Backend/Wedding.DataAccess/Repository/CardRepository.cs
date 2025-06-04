using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Microsoft.EntityFrameworkCore;


namespace Wedding.DataAccess.Repository
{
    public class CardRepository : Repository<Card>, ICardRepository
    {

        private readonly ApplicationDbContext _context;

        public CardRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Card card)
        {
            _context.Cards.Update(card);
        }
        public void UpdateRange(IEnumerable<Card> cards)
        {
            _context.Cards.UpdateRange(cards);
        }
        public async Task<Card?> GetById(Guid cardId)
        {
            return await _context.Cards.FirstOrDefaultAsync(x => x.Id == cardId);
        }
    }
}
