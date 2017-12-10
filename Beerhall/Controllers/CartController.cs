using System.Linq;
using Beerhall.Filters;
using Microsoft.AspNetCore.Mvc;
using Beerhall.Models.Domain;
using Beerhall.Models.CartViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;

namespace Beerhall.Controllers
{
    [ServiceFilter(typeof(CartSessionFilter))]
    public class CartController : Controller
    {
        private readonly IBeerRepository _beerRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ICustomerRepository _customerRepository;

        public CartController(IBeerRepository beerRepository, ILocationRepository locationRepository, ICustomerRepository customerRepository)
        {
            _beerRepository = beerRepository;
            _locationRepository = locationRepository;
            _customerRepository = customerRepository;
        }

        public IActionResult Index(Cart cart)
        {
            ViewData["Total"] = cart.TotalValue;
            return View(cart.CartLines.Select(c => new IndexViewModel(c)).ToList());
        }

        [HttpPost]
        public IActionResult Add(Cart cart, int id, int quantity = 1)
        {
            try
            {
                Beer product = _beerRepository.GetBy(id);
                if (product != null)
                {
                    cart.AddLine(product, quantity);
                    TempData["message"] = $"{quantity} x {product.Name} was added to your cart";
                }
            }
            catch
            {
                TempData["error"] = "Sorry, something went wrong, the product could not be added to your cart...";
            }
            return RedirectToAction("Index", "Store");
        }

        [HttpPost]
        public IActionResult Remove(Cart cart, int id)
        {
            try
            {
                Beer product = _beerRepository.GetBy(id);
                cart.RemoveLine(product);
                TempData["message"] = $"{product.Name} was removed from your cart";
            }
            catch
            {
                TempData["error"] = "Sorry, something went wrong, the product was not removed from your cart...";
            }
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "Customer")]
        public IActionResult Checkout(Cart cart)
        {
            if (cart.NumberOfItems == 0)
                return RedirectToAction("Index", "Store");
            IEnumerable<Location> locations = _locationRepository.GetAll().OrderBy(l => l.Name).ToList();
            return View(new CheckOutViewModel(locations, new ShippingViewModel()));
        }

        [HttpPost, Authorize(Policy = "Customer")]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult Checkout(Customer customer, Cart cart, [Bind(Prefix = "ShippingViewModel")]ShippingViewModel shippingVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (cart.NumberOfItems == 0)
                        return RedirectToAction("Index");
                    Location location = _locationRepository.GetBy(shippingVm.PostalCode);
                    customer.PlaceOrder(cart, shippingVm.DeliveryDate, shippingVm.Giftwrapping, shippingVm.Street, location);
                    _customerRepository.SaveChanges();
                    cart.Clear();
                    TempData["message"] = "Thank you for your order!";
                    return RedirectToAction("Index", "Store");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            IEnumerable<Location> locations = _locationRepository.GetAll().OrderBy(l => l.Name);
            return View(new CheckOutViewModel(locations, shippingVm));
        }
    }
}