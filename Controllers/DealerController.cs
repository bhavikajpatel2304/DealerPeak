using DealerPeak.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace DealerPeak.Controllers
{
    public class DealerController : Controller
    {
        private DealerPeakDbContext context { get; set; }

        public DealerController(DealerPeakDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ContactSortParm = sortOrder == "contact" ? "contact_desc" : "contact";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var dealers = from d in context.Dealers
                          select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                dealers = dealers.Where(d => Convert.ToString(d.DealerName).Contains(searchString)
                                          || Convert.ToString(d.DealerContact).Contains(searchString)
                                          || Convert.ToString(d.DealerEmail).Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    dealers = dealers.OrderByDescending(d => d.DealerName);
                    break;
                case "contact":
                    dealers = dealers.OrderBy(d => d.DealerContact);
                    break;
                case "contact_desc":
                    dealers = dealers.OrderByDescending(d => d.DealerContact);
                    break;
                case "email":
                    dealers = dealers.OrderBy(d => d.DealerEmail);
                    break;
                case "email_desc":
                    dealers = dealers.OrderByDescending(d => d.DealerEmail);
                    break;
                default:
                    dealers = dealers.OrderBy(d => d.DealerName);
                    break;
            }
            //int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(dealers.ToPagedList(pageNumber, 5));
        }

        [HttpGet]
        public IActionResult AddDealer()
        {
            ViewData["action"] = "Add";
            return View("EditDealer", new Dealer());
        }

        [HttpGet]
        public IActionResult EditDealer(int id)
        {
            ViewData["action"] = "Edit";
            var dealer = context.Dealers.Find(id);
            return View(dealer);
        }

        [HttpPost]
        public IActionResult EditDealer(Dealer dealer)
        {
            //check weather contact exists on DB or not
            if (TempData["okDealerContact"] == null)
            {
                string msg = Check.DealerContactExists(context, dealer.DealerContact);
                if (!String.IsNullOrEmpty(msg))
                {
                    ModelState.AddModelError(nameof(dealer.DealerContact), msg);
                }
            }

            //check weather email exists on DB or not
            if (TempData["okDealerEmail"] == null)
            {
                string msg = Check.DealerEmailExists(context, dealer.DealerEmail);
                if (!String.IsNullOrEmpty(msg))
                {
                    ModelState.AddModelError(nameof(dealer.DealerEmail), msg);
                }
            }

            if (ModelState.IsValid)
            {
                //new dealer added
                if (dealer.DealerId == 0)
                {
                    context.Dealers.Add(dealer);
                    context.SaveChanges();
                }
                else
                {
                    //updating an existing dealer
                    dealer.DateAdded = DateTime.UtcNow;
                    context.Dealers.Update(dealer);
                    context.SaveChanges();
                }
                return RedirectToAction("Index", "Dealer");
            }
            else {
                //return RedirectToAction("EditDealer", "Dealer");
                return View(dealer);
            }
        }

        [HttpGet]
        public IActionResult DeleteDealer(int id)
        {
            var dealer = context.Dealers.Find(id);
            return View(dealer);
        }

        [HttpPost]
        public IActionResult DeleteDealer(Dealer dealer)
        {
            if (dealer.DealerId > 0)
            { 
                context.Dealers.Remove(dealer);
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Dealer");
        }
    }
}
