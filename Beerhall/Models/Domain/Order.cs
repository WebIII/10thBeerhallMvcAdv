using System;
using System.Collections.Generic;
using System.Linq;

namespace Beerhall.Models.Domain
{
    public class Order
    {
        #region Fields
        private DateTime? _deliveryDate;
        private string _street;
        private Location _location;
        #endregion

        #region Properties
        public int OrderId { get; private set; } // required for EF
        public DateTime? DeliveryDate {
            get { return _deliveryDate; }
            private set {
                if (value.HasValue)
                {
                    if (DateTime.Today.AddDays(3) > value.Value)
                        throw new ArgumentException("Date of delivery must at least be three days after placing order");
                    if (value.Value.DayOfWeek == DayOfWeek.Sunday)
                        throw new ArgumentException("Sundays are not valid delivery days");
                }
                _deliveryDate = value;
            }
        }

        public DateTime OrderDate { get; private set; }
        public bool Giftwrapping { get; private set; }
        public string Street {
            get { return _street; }
            private set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Street is required");
                _street = value;
            }
        }

        public Location Location {
            get { return _location; }
            private set {
                _location = value ?? throw new ArgumentException("Location is required");
            }
        }

        public ICollection<OrderLine> OrderLines { get; private set; }
        public decimal Total { get { return OrderLines.Sum(o => o.Price * o.Quantity); } }

        #endregion

        #region Constructors
        private Order()
        {
            OrderLines = new HashSet<OrderLine>();
            OrderDate = DateTime.Today;
        }

        public Order(Cart cart, DateTime? deliveryDate, bool giftwrapping, string street, Location location) : this()
        {
            if (cart.NumberOfItems == 0)
                throw new ArgumentException("An order requires a non empty cart");
            foreach (CartLine line in cart.CartLines)
            {
                OrderLines.Add(new OrderLine
                {
                    Product = line.Product,
                    Price = line.Product.Price,
                    Quantity = line.Quantity
                });
            }

            DeliveryDate = deliveryDate;
            Giftwrapping = giftwrapping;
            Street = street;
            Location = location;
        }
        #endregion
    }
}
