using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Wedding.DataAccess.Repository;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Customer student)
    {
        _context.Customers.Update(student);
    }

    public void UpdateRange(IEnumerable<Customer> students)
    {
        _context.Customers.UpdateRange(students);
    }
    public async Task<Customer> AddAsync(Customer student)
    {
        var entityEntry = await _context.Customers.AddAsync(student);
        return entityEntry.Entity;
    }

    public async Task<Customer?> GetByUserId(string id)
    {
        return await _context.Customers.Include("ApplicationUser").FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task<Customer?> GetById(Guid id)
    {
        return await _context.Customers.Include("ApplicationUser").FirstOrDefaultAsync(x => x.CustomerId == id);

    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}
