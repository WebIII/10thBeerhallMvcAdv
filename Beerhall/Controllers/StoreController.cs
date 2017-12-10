using System.Linq;
using Beerhall.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Beerhall.Controllers
{
    public class StoreController : Controller
    {
        private readonly IBeerRepository _beerRepository;

        public StoreController(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public ActionResult Index()
        {
            return View(_beerRepository.GetAll().OrderBy(b => b.Name).ToList());
        }
    }
}