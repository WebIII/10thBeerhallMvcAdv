using System;
using System.Collections.Generic;
using Beerhall.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Beerhall.Models.CartViewModels
{
    public class CheckOutViewModel
    {
        public SelectList Locations { get; }
        public ShippingViewModel ShippingViewModel { get; set; }
        public CheckOutViewModel(IEnumerable<Location> locations, ShippingViewModel shippingViewModel)
        {
            Locations = new SelectList(locations,
                nameof(Location.PostalCode),
                nameof(Location.Name),
                shippingViewModel?.PostalCode);
            ShippingViewModel = shippingViewModel;
        }
    }

    public class ShippingViewModel
    {
        public DateTime? DeliveryDate { get; set; }
        public bool Giftwrapping { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}
