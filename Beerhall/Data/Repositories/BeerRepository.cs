using System.Collections.Generic;
using System.Linq;
using Beerhall.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Beerhall.Data.Repositories
{
    public class BeerRepository : IBeerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Beer> _beers;

        public BeerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _beers = _dbContext.Beers;
        }

        public IEnumerable<Beer> GetAll()
        {
            return _beers.ToList();
        }

        public Beer GetBy(int beerId)
        {
            return _beers.SingleOrDefault(b => b.BeerId == beerId);
        }
    }
}
