using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Beerhall.Models.Domain;

namespace Beerhall.Filters
{
    public class CartSessionFilter : ActionFilterAttribute
    {
        private readonly IBeerRepository _beerRepository;
        private Cart _cart;

        public CartSessionFilter(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _cart = ReadCartFromSession(context.HttpContext);
            context.ActionArguments["cart"] = _cart;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            WriteCartToSession(_cart, context.HttpContext);
            base.OnActionExecuted(context);
        }

        private Cart ReadCartFromSession(HttpContext context)
        {
            Cart cart = context.Session.GetString("cart") == null ?
                new Cart() : JsonConvert.DeserializeObject<Cart>(context.Session.GetString("cart"));
            foreach (var l in cart.CartLines)
                l.Product = _beerRepository.GetBy(l.Product.BeerId);
            return cart;
        }

        private void WriteCartToSession(Cart cart, HttpContext context)
        {
            context.Session.SetString("cart", JsonConvert.SerializeObject(cart));
        }
    }
}

