using System.Collections.Generic;

namespace Beerhall.Models.Domain
{
    public interface ILocationRepository
    {
        Location GetBy(string postalCode);
        IEnumerable<Location> GetAll();
    }
}
