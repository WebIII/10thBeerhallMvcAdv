namespace Beerhall.Models.Domain
{
    public interface ICustomerRepository
    {
        Customer GetBy(string email);
        void SaveChanges();
    }
}
