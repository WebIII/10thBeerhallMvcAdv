using System.Linq;
using Beerhall.Filters;
using Microsoft.AspNetCore.Mvc;
using Beerhall.Models.Domain;
using Beerhall.Models.CartViewModels;

namespace Beerhall.Controllers
{
    [ServiceFilter(typeof(CartSessionFilter))]
    public class CartController : Controller
    {
        private readonly IBeerRepository _beerRepository;

        public CartController(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
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
    }
}