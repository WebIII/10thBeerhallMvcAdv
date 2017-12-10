using Beerhall.Models.Domain;
using System;
using System.Linq;
using Beerhall.Tests.Data;
using Xunit;

namespace Beerhall.Tests.Models.Domain
{
    public class OrderTest
    {
        private readonly Cart _cart;
        private readonly Beer _beer1;
        private readonly Beer _beer2;
        private readonly DateTime _futureDayNotSunday;
        private readonly DateTime _futureDaySunday;
        private readonly Location _location;

        public OrderTest()
        {
            DummyApplicationDbContext context = new DummyApplicationDbContext();
            _beer1 = context.BavikPils;
            _beer2 = context.Wittekerke;
            _location = context.Bavikhove;
            _cart = new Cart();
            _cart.AddLine(_beer1, 10);
            _cart.AddLine(_beer2, 1);
            _futureDaySunday = DateTime.Today.AddDays(14).AddDays(-(DateTime.Today.AddDays(14).DayOfWeek - DayOfWeek.Sunday));
            _futureDayNotSunday = DateTime.Today.AddDays(14).AddDays(-(DateTime.Today.AddDays(14).DayOfWeek - DayOfWeek.Monday));
        }

        [Fact]
        public void NewOrder_NullOrEmptyStreet_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Order(_cart, _futureDayNotSunday, true, null, _location));
            Assert.Throws<ArgumentException>(() => new Order(_cart, _futureDayNotSunday, true, " ", _location));
        }

        [Fact]
        public void NewOrder_NullLocation_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Order(_cart, _futureDayNotSunday, true, "Street 1", null));
        }

        [Fact]
        public void NewOrder_DeliveryDateIsSunday_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Order(_cart, _futureDaySunday, true, "Street 1", _location));
        }

        [Fact]
        public void NewOrder_DeliveryDateIsTooSoon_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Order(_cart, DateTime.Today.AddDays(1).DayOfWeek == DayOfWeek.Sunday ? DateTime.Today.AddDays(2) : DateTime.Today.AddDays(1), true, "Street 1", _location));
        }

        [Fact]
        public void NewOrder_EmptyCart_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Order(new Cart(), _futureDayNotSunday, true, "Street 1", _location));
        }

        [Fact]
        public void NewOrder_ValidData_CreatesOrder()
        {
            var order = new Order(_cart, _futureDayNotSunday, true, "Street 1", _location);
            Assert.Equal(2, order.OrderLines.Count);
            Assert.True(order.OrderLines.Select(ol => ol.Product).Contains(_beer1));
            Assert.True(order.OrderLines.Select(ol => ol.Product).Contains(_beer2));
            Assert.Equal(DateTime.Today, order.OrderDate);
            Assert.Equal(_futureDayNotSunday, order.DeliveryDate);
            Assert.Equal("Street 1", order.Street);
            Assert.Equal(_location, order.Location);
        }
    }
}