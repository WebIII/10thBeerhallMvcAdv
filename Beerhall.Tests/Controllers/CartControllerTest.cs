using System.Linq;
using Moq;
using System.Collections.Generic;
using Beerhall.Controllers;
using Beerhall.Models.Domain;
using Beerhall.Models.CartViewModels;
using Beerhall.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace Beerhall.Tests.Controllers
{
    public class CartControllerTest
    {
        private readonly CartController _controller;
        private readonly Cart _cart;
        private readonly DummyApplicationDbContext _context;
        private readonly Mock<IBeerRepository> _beerRepository;

        public CartControllerTest()
        {
            _context = new DummyApplicationDbContext();
            _beerRepository = new Mock<IBeerRepository>();
            _beerRepository.Setup(b => b.GetAll()).Returns(_context.Beers);
            var locationRepository = new Mock<ILocationRepository>();
            locationRepository.Setup(b => b.GetAll()).Returns(_context.Locations);

            _controller = new CartController(_beerRepository.Object, locationRepository.Object)
            {
                TempData = new Mock<ITempDataDictionary>().Object
            };

            _cart = new Cart();
            _cart.AddLine(_context.Wittekerke, 5); // Beer with BeerId = 2
        }

        #region Index
        [Fact]
        public void Index_EmptyCart_PassesCartToDefaultView()
        {
            var actionResult = _controller.Index(new Cart()) as ViewResult;
            var cartLines = actionResult?.Model as IEnumerable<IndexViewModel>;
            Assert.Equal(0, cartLines.Count());
            Assert.Null(actionResult.ViewName);
        }

        [Fact]
        public void Index_NonEmptyCart_PassesCartToDefaultView()
        {
            var actionResult = _controller.Index(_cart) as ViewResult;
            var cartLines = actionResult?.Model as IEnumerable<IndexViewModel>;
            Assert.Equal(1, cartLines.Count());
            Assert.Null(actionResult.ViewName);
        }

        [Fact]
        public void Index_NonEmptyCart_StoresTotalInViewData()
        {
            var actionResult = _controller.Index(_cart) as ViewResult;
            Assert.Equal(10M, actionResult?.ViewData["Total"]);
        }
        #endregion

        #region Add
        [Fact]
        public void Add_RedirectsToActionIndexInStore()
        {
            var actionResult = _controller.Add(_cart, 1) as RedirectToActionResult;
            Assert.Equal("Index", actionResult?.ActionName);
            Assert.Equal("Store", actionResult?.ControllerName);
        }

        [Fact]
        public void Add_AddsProductToCart()
        {
            _beerRepository.Setup(b => b.GetBy(1)).Returns(_context.BavikPils);
            _controller.Add(_cart, 1, 4);
            Assert.Equal(2, _cart.NumberOfItems);
        }

        #endregion

        #region Remove
        [Fact]
        public void Remove_RedirectsToActionIndexInDefaultController()
        {
            _beerRepository.Setup(b => b.GetBy(2)).Returns(_context.Wittekerke);
            var actionResult = _controller.Remove(_cart, 2) as RedirectToActionResult;
            Assert.Equal("Index", actionResult?.ActionName);
            Assert.Equal(null, actionResult?.ControllerName);
        }

        [Fact]
        public void Remove_RemovesProductFromCart()
        {
            _beerRepository.Setup(b => b.GetBy(2)).Returns(_context.Wittekerke);
            _controller.Remove(_cart, 2);
            Assert.Equal(0, _cart.NumberOfItems);
        }
        #endregion

        #region Checkout HttpGet

        [Fact]
        public void Checkout_EmptyCart_RedirectsToIndexOfStore()
        {
            var actionResult = _controller.Checkout(new Cart()) as RedirectToActionResult;
            Assert.Equal("Index", actionResult?.ActionName);
            Assert.Equal("Store", actionResult?.ControllerName);
        }

        [Fact]
        public void Checkout_NonEmptyCart_PassesACheckOutViewModelInViewResultModel()
        {
            var actionResult = _controller.Checkout(_cart) as ViewResult;
            var model = actionResult?.Model as CheckOutViewModel;
            Assert.Null(model.ShippingViewModel.DeliveryDate);
            Assert.Null(model.ShippingViewModel.PostalCode);
            Assert.Null(model.ShippingViewModel.Street);
            Assert.False(model.ShippingViewModel.Giftwrapping);
            Assert.Equal(3, model.Locations.Count());
        }

        #endregion

    }
}