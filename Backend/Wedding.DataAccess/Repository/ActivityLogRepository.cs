using Microsoft.EntityFrameworkCore;
using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class ActivityLogRepository : Repository<ActivityLog>, IActivityLogRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityLogRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(ActivityLog activityLog)
    {
        _context.ActivityLogs.Update(activityLog);
    }
    
    public void UpdateRange(IEnumerable<ActivityLog> activityLogs)
    {
        _context.ActivityLogs.UpdateRange(activityLogs);
    }
    
    public async Task<ActivityLog?> GetById(Guid id)
    {
        return await _context.ActivityLogs.FirstOrDefaultAsync(x => x.LogId == id);

    }
    
    public async Task<ActivityLog> AddAsync(ActivityLog activityLog)
    {
        var entityEntry = await _context.ActivityLogs.AddAsync(activityLog);
        return entityEntry.Entity;
    }
}