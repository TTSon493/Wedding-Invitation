using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IEventPhotoRepository : IRepository<EventPhoto>
{
    void Update(EventPhoto eventoPhoto);
    void UpdateRange(IEnumerable<EventPhoto> eventoPhotos);
    Task<EventPhoto?> GetById(Guid id);
    Task<EventPhoto> AddAsync(EventPhoto eventoPhoto);
}