using System.Collections.Generic;

namespace Beerhall.Models.Domain
{
    public interface IBeerRepository
    {
        IEnumerable<Beer> GetAll();
        Beer GetBy(int beerId);
    }
}