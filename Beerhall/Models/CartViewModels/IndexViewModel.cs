using Beerhall.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Beerhall.Models.CartViewModels
{
    public class IndexViewModel
    {
        [HiddenInput]
        public int BeerId { get; }

        public int Quantity { get; }

        public string Beer { get; }

        public decimal Price { get; }

        public decimal SubTotal { get; }

        public IndexViewModel(CartLine cartLine)
        {
            BeerId = cartLine.Product.BeerId;
            Quantity = cartLine.Quantity;
            Beer = cartLine.Product.Name;
            Price = cartLine.Product.Price;
            SubTotal = cartLine.Total;
        }
    }
}
