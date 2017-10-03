using Beerhall.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beerhall.Data.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly DbSet<Location> _locations;

        public LocationRepository(ApplicationDbContext dbContext)
        {
            _locations = dbContext.Locations;
        }

        public Location GetBy(string postalCode)
        {
            return _locations.SingleOrDefault(l => l.PostalCode == postalCode);
        }

        public IEnumerable<Location> GetAll()
        {
            return _locations.ToList();
        }
    }
}
