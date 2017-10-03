using System;
using Xunit;
using Beerhall.Models.Domain;
using System.Linq;

namespace Beerhall.Tests.Models.Domain
{
    public class BrewerTest
    {
        private readonly Brewer _bockor;

        public BrewerTest()
        {
            _bockor = new Brewer("Bockor");
            _bockor.AddBeer("Omer");
            _bockor.AddBeer("Bellegems Bruin");
        }

        #region Constructor
        [Fact]
        public void NewBrewer_CorrectName_CreatesBrewer()
        {
            Brewer brewer = new Brewer("Rodenbach");
            Assert.Equal("Rodenbach", brewer.Name);
            Assert.Null(brewer.Turnover);
            Assert.Equal(0, brewer.NrOfBeers);
            Assert.Null(brewer.Location);
            Assert.Null(brewer.Street);
            Assert.Equal(0, brewer.BrewerId);
        }

        [Fact]
        public void NewBrewer_AddressSet_CreatesBrewer()
        {
            Location veurne = new Location { Name = "Veurne", PostalCode = "8630" };
            Brewer brouwer = new Brewer("Bachten de Kupe", veurne, "Kerkstraat 20") { Turnover = 20000 };
            Assert.Equal("Bachten de Kupe", brouwer.Name);
            Assert.Equal("Veurne", brouwer.Location.Name);
            Assert.Equal("Kerkstraat 20", brouwer.Street);
            Assert.Equal(20000, brouwer.Turnover);
        }

        [Fact]
        public void NewBrewer_NegativeTurnover_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new Brewer("Rodenbach") { Turnover = -2000 });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(" \t \n \r \t   ")]
        [InlineData("01234567890123456789012345678901234567890123456789*")]
        public void NewBrewer_NameIncorrect_ThrowsException(string name)
        {
            Assert.Throws<ArgumentException>(() => new Brewer(name));
        }
        #endregion

        #region AddBeer
        [Fact]
        public void AddBeer_NewBeer_AddsABeer()
        {
            int nrOfBeersBeforeAdd = _bockor.NrOfBeers;
            _bockor.AddBeer("HoGent beer", 55.0D);
            Assert.Equal(nrOfBeersBeforeAdd + 1, _bockor.NrOfBeers);
        }

        [Fact]
        public void AddBeer_BeerThatHasADuplicateName_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _bockor.AddBeer("Omer"));
        }

        #endregion

        #region DeleteBeer
        [Fact]
        public void DeleteBeer_ExistingBeer_DeletesTheBeer()
        {
            int nrOfBeersBeforeDelete = _bockor.NrOfBeers;
            Beer aBeer = _bockor.Beers.First();
            _bockor.DeleteBier(aBeer);
            Assert.Equal(nrOfBeersBeforeDelete - 1, _bockor.NrOfBeers);
        }

        [Fact]
        public void DeleteBeer_NonExistingBeer_ThrowsException()
        {
            Beer aBeer = new Beer("Just a beer");
            Assert.Throws<ArgumentException>(() => _bockor.DeleteBier(aBeer));
        }
        #endregion

    }
}
