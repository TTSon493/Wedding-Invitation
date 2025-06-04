using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IActivityLogRepository : IRepository<ActivityLog>
{
    void Update(ActivityLog activityLog);
    void UpdateRange(IEnumerable<ActivityLog> activityLogs);
    Task<ActivityLog?> GetById(Guid id);
    Task<ActivityLog> AddAsync(ActivityLog activityLog);
}