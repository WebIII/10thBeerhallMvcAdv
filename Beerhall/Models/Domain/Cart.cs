using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Beerhall.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Cart
    {
        #region Properties
        [JsonProperty]
        private readonly IList<CartLine> _lines = new List<CartLine>();
        public IEnumerable<CartLine> CartLines => _lines.AsEnumerable();
        public int NumberOfItems => _lines.Count;
        public decimal TotalValue {
            get { return _lines.Sum(l => l.Product.Price * l.Quantity); }
        }
        #endregion

        #region Methods
        public void AddLine(Beer product, int quantity)
        {
            CartLine line = _lines.SingleOrDefault(l => l.Product.BeerId == product.BeerId);
            if (line == null)
                _lines.Add(new CartLine { Product = product, Quantity = quantity });
            else
                line.Quantity += quantity;
        }

        public void RemoveLine(Beer product)
        {
            CartLine line = _lines.SingleOrDefault(l => l.Product.BeerId == product.BeerId);
            if (line != null)
                _lines.Remove(line);
        }

        public void Clear()
        {
            _lines.Clear();
        }
        #endregion
    }
}