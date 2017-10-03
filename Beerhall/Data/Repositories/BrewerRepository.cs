using Beerhall.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Beerhall.Data.Repositories
{
    public class BrewerRepository : IBrewerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Brewer> _brewers;

        public BrewerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _brewers = dbContext.Brewers;
        }

        public Brewer GetBy(int brewerId)
        {
            return _brewers.Include(b => b.Location).SingleOrDefault(b => b.BrewerId == brewerId);
        }

        public IEnumerable<Brewer> GetAll()
        {
            return _brewers.Include(b => b.Location).Include(b => b.Beers).ToList();
        }

        public void Add(Brewer brewer)
        {
            _brewers.Add(brewer);
        }

        public void Delete(Brewer brewer)
        {
            _brewers.Remove(brewer);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
