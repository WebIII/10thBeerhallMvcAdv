using System;
using System.Collections.Generic;

namespace Beerhall.Models.Domain
{
    public class Customer
    {
        #region Properties
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Street { get; set; }
        public Location Location { get; set; }
        public ICollection<Order> Orders { get; set; }
        #endregion

        #region Methods
        public Customer()
        {
            Orders = new List<Order>();
        }

        public void PlaceOrder(Cart cart, DateTime? deliveryDate, bool giftwrapping, string shippingStreet, Location shippingCity)
        {
            Orders.Add(new Order(cart, deliveryDate, giftwrapping, shippingStreet, shippingCity));
        }
        #endregion
    }
}
