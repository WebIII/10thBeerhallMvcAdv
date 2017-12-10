using Beerhall.Models.Domain;
using System.Linq;
using Xunit;

namespace Beerhall.Tests.Models.Domain
{
    public class CartTest
    {
        private readonly Cart _cart;
        private readonly Beer _beer1;
        private readonly Beer _beer2;

        public CartTest()
        {
            _beer1 = new Beer("Beer1") { BeerId = 1, Price = 1.5M };
            _beer2 = new Beer("Beer2") { BeerId = 2, Price = 2 };
            _cart = new Cart();
        }

        [Fact]
        public void NewCart_CreatesEmptyCart()
        {
            Assert.Equal(0, _cart.NumberOfItems);
        }

        [Fact]
        public void AddLine_AddsProductToCart()
        {
            _cart.AddLine(_beer1, 1);
            _cart.AddLine(_beer2, 10);
            Assert.Equal(2, _cart.NumberOfItems);
            Assert.Equal(1, _cart.CartLines.First(l => l.Product.Equals(_beer1)).Quantity);
            Assert.Equal(10, _cart.CartLines.First(l => l.Product.Equals(_beer2)).Quantity);
        }

        [Fact]
        public void AddLine_ProductThatIsAlreadyInCart_AdjustsQuantityForThatProduct()
        {
            _cart.AddLine(_beer1, 1);
            _cart.AddLine(_beer2, 10);
            _cart.AddLine(_beer1, 3);
            Assert.Equal(2, _cart.NumberOfItems);
            Assert.Equal(4, _cart.CartLines.First(l => l.Product.Equals(_beer1)).Quantity);
            Assert.Equal(10, _cart.CartLines.First(l => l.Product.Equals(_beer2)).Quantity);
        }

        [Fact]
        public void RemoveLine_ProductThatIsInCart_RemovesTheProductFromTheCart()
        {
            _cart.AddLine(_beer1, 1);
            _cart.AddLine(_beer2, 10);
            _cart.RemoveLine(_beer2);
            Assert.Equal(1, _cart.NumberOfItems);
            Assert.Equal(1, _cart.CartLines.First(l => l.Product.Equals(_beer1)).Quantity);
        }

        [Fact]
        public void RemoveLine_ProductThatIsNotInCart_DoesNotChangeTheCart()
        {
            _cart.AddLine(_beer2, 10);
            _cart.RemoveLine(_beer1);
            Assert.Equal(1, _cart.NumberOfItems);
            Assert.Equal(10, _cart.CartLines.First(l => l.Product.Equals(_beer2)).Quantity);
        }

        [Fact]
        public void Clear_CartThatContainsProducts_ClearsTheCart()
        {
            _cart.AddLine(_beer1, 1);
            _cart.AddLine(_beer2, 10);
            _cart.AddLine(_beer1, 3);
            _cart.Clear();
            Assert.Equal(0, _cart.NumberOfItems);
        }

        [Fact]
        public void Clear_EpmtyCart_DoesNotChangeTheCart()
        {
            _cart.AddLine(_beer1, 1);
            _cart.AddLine(_beer2, 10);
            _cart.AddLine(_beer1, 3);
            _cart.Clear();
            Assert.Equal(0, _cart.NumberOfItems);
        }

        [Fact]
        public void TotalValue_CartWithProducts_ReturnsTotalPrice()
        {
            _cart.AddLine(_beer1, 10);
            _cart.AddLine(_beer2, 5);
            Assert.Equal(25, _cart.TotalValue, 2);
        }

        [Fact]
        public void TotalValue_EmptyCart_ReturnsZero()
        {
            Assert.Equal(0, _cart.TotalValue, 2);
        }
    }
}
