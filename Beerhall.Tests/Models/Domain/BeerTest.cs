using System;
using Xunit;
using Beerhall.Models.Domain;

namespace Beerhall.Tests.Models.Domain
{
    public class BeerTest
    {
        #region Constructor
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(" \t \n \r \t   ")]
        public void NewBeer_WrongName_ThrowsException(string name)
        {
            Assert.Throws<ArgumentException>(() => new Beer(name));
        }

        #endregion

        #region AlcholKnown
        [Fact]
        public void AlcoholKnown_AlcoholByVolumeSet_ReturnsTrue()
        {
            Beer beer = new Beer("New beer") { AlcoholByVolume = 8.5D };
            Assert.True(beer.AlcoholKnown);
        }

        [Fact]
        public void AlcoholKnown_AlcoholByVolumeNotSet_ReturnsFalse()
        {
            Beer beer = new Beer("New beer");
            Assert.False(beer.AlcoholKnown);
        }
        #endregion
    }
}
