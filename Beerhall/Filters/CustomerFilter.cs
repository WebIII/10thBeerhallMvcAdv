using Beerhall.Models.Domain;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beerhall.Filters
{
    public class CustomerFilter : ActionFilterAttribute
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerFilter(ICustomerRepository customerRespoitory)
        {
            _customerRepository = customerRespoitory;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments["customer"] = context.HttpContext.User.Identity.IsAuthenticated ? _customerRepository.GetBy(context.HttpContext.User.Identity.Name) : null;
            base.OnActionExecuting(context);
        }
    }
}