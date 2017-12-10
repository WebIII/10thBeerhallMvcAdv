using System.Collections.Generic;
using Beerhall.Controllers;
using Beerhall.Models.Domain;
using Beerhall.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Beerhall.Tests.Controllers
{
    public class StoreControllerTest
    {
        private readonly StoreController _controller;
        private readonly Mock<IBeerRepository> _beerRepository;
        private readonly DummyApplicationDbContext _dummyContext;

        public StoreControllerTest()
        {
            _dummyContext = new DummyApplicationDbContext();
            _beerRepository = new Mock<IBeerRepository>();
            _controller = new StoreController(_beerRepository.Object);
        }

        [Fact]
        public void Index_PassesOrderedListOfBeersInViewResultModel()
        {
            _beerRepository.Setup(m => m.GetAll()).Returns(_dummyContext.Beers);
            var actionResult = _controller.Index() as ViewResult;
            var beersInModel = actionResult?.Model as IList<Beer>;
            Assert.Equal(3, beersInModel?.Count);
            Assert.Equal("Bavik Pils", beersInModel?[0].Name);
            Assert.Equal("Duvel", beersInModel?[1].Name);
            Assert.Equal("Wittekerke", beersInModel?[2].Name);
        }
    }
}