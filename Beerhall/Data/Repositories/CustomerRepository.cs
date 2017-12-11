using System.Linq;
using Beerhall.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Beerhall.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbSet<Customer> _customers;
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _customers = _dbContext.Customers;
        }

        public Customer GetBy(string email)
        {
            return _customers.Include(c => c.Location).SingleOrDefault(c => c.Email == email);
        }

        public void Add(Customer customer)
        {
            _customers.Add(customer);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}