using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Beerhall.Models.Domain;
using Beerhall.Models.CartViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Beerhall.Controllers
{
    public class CartController : Controller
    {
        private readonly IBeerRepository _beerRepository;

        public CartController(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public IActionResult Index()
        {
            Cart cart = ReadCartFromSession();
            ViewData["Total"] = cart.TotalValue;
            return View(cart.CartLines.Select(c => new IndexViewModel(c)).ToList());
        }

        [HttpPost]
        public IActionResult Add(int id, int quantity = 1)
        {
            try
            {
                Cart cart = ReadCartFromSession();
                Beer product = _beerRepository.GetBy(id);
                if (product != null)
                {
                    cart.AddLine(product, quantity);
                    TempData["message"] = $"{quantity} x {product.Name} was added to your cart";
                    WriteCartToSession(cart);
                }
            }
            catch
            {
                TempData["error"] = "Sorry, something went wrong, the product could not be added to your cart...";
            }
            return RedirectToAction("Index", "Store");
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            try
            {
                Cart cart = ReadCartFromSession();
                Beer product = _beerRepository.GetBy(id);
                cart.RemoveLine(product);
                TempData["message"] = $"{product.Name} was removed from your cart";
                WriteCartToSession(cart);
            }
            catch
            {
                TempData["error"] = "Sorry, something went wrong, the product was not removed from your cart...";
            }
            return RedirectToAction("Index");
        }

        private Cart ReadCartFromSession()
        {
            Cart cart = HttpContext.Session.GetString("cart") == null
                ? new Cart()
                : JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("cart"));
            foreach (var l in cart.CartLines)
                l.Product = _beerRepository.GetBy(l.Product.BeerId);
            return cart;
        }

        private void WriteCartToSession(Cart cart)
        {
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
        }
    }
}