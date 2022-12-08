using DealerPeak.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using X.PagedList;

namespace DealerPeak.Controllers
{
    public class UserController : Controller
    {
        private DealerPeakDbContext context { get; set; }
        public UserController(DealerPeakDbContext dpc)
        {
            this.context = dpc;
        }

        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "fname_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "lname" ? "lname_desc" : "lname";
            ViewBag.CitySortParm = sortOrder == "city" ? "city_desc" : "city";
            ViewBag.ContactSortParm = sortOrder == "contact" ? "contact_desc" : "contact";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.ActiveUserSortParm = sortOrder == "activeUser" ? "activeUser_desc" : "activeUser";
            ViewBag.LoginTypeSortParm = sortOrder == "loginType" ? "loginType_desc" : "loginType";
            ViewBag.AddressSortParm = sortOrder == "address" ? "address_desc" : "address";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var users = from u in context.Users
                        where u.LoginType.Trim().ToLower().Contains("user")
                         select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(a => Convert.ToString(a.FirstName).Contains(searchString)
                                          || Convert.ToString(a.LastName).Contains(searchString)
                                          || Convert.ToString(a.Address).Contains(searchString)
                                          || Convert.ToString(a.City).Contains(searchString)
                                          || Convert.ToString(a.Contact).Contains(searchString)
                                          || Convert.ToString(a.Email).Contains(searchString)
                                          || (a.IsActive == true ? "Yes" : "No").Contains(searchString)
                                          || Convert.ToString(a.LoginType).Contains(searchString));
            }

            switch (sortOrder)
            {
                case "fname_desc":
                    users = users.OrderByDescending(a => a.FirstName);
                    break;
                case "lname":
                    users = users.OrderBy(a => a.LastName);
                    break;
                case "lname_desc":
                    users = users.OrderByDescending(a => a.LastName);
                    break;
                case "city":
                    users = users.OrderBy(a => a.City);
                    break;
                case "city_desc":
                    users = users.OrderByDescending(a => a.City);
                    break;
                case "address":
                    users = users.OrderBy(a => a.Address);
                    break;
                case "address_desc":
                    users = users.OrderByDescending(a => a.Address);
                    break;
                case "contact":
                    users = users.OrderBy(a => a.Contact);
                    break;
                case "contact_desc":
                    users = users.OrderByDescending(a => a.Contact);
                    break;
                case "email":
                    users = users.OrderBy(a => a.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(a => a.Email);
                    break;
                case "activeUser":
                    users = users.OrderBy(a => a.IsActive);
                    break;
                case "activeUser_desc":
                    users = users.OrderByDescending(a => a.IsActive);
                    break;
                case "loginType":
                    users = users.OrderBy(a => a.LoginType);
                    break;
                case "loginType_desc":
                    users = users.OrderByDescending(a => a.LoginType);
                    break;
                default:
                    users = users.OrderBy(a => a.FirstName);
                    break;
            }
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, 5));
        }

        public IActionResult Admin(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "fname_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "lname" ? "lname_desc" : "lname";
            ViewBag.ContactSortParm = sortOrder == "contact" ? "contact_desc" : "contact";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.ActiveUserSortParm = sortOrder == "activeUser" ? "activeUser_desc" : "activeUser";
            
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var admins = from a in context.Users where a.LoginType.Contains("Admin")
                         select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                admins = admins.Where(a => Convert.ToString(a.FirstName).Contains(searchString)
                                          || Convert.ToString(a.LastName).Contains(searchString)
                                          || Convert.ToString(a.Contact).Contains(searchString)
                                          || Convert.ToString(a.Email).Contains(searchString)
                                          || (a.IsActive == true ? "Yes" : "No").Contains(searchString));
            }

            switch (sortOrder)
            {
                case "fname_desc":
                    admins = admins.OrderByDescending(a => a.FirstName);
                    break;
                case "lname":
                    admins = admins.OrderBy(a => a.LastName);
                    break;
                case "lname_desc":
                    admins = admins.OrderByDescending(a => a.LastName);
                    break;
                case "contact":
                    admins = admins.OrderBy(a => a.Contact);
                    break;
                case "contact_desc":
                    admins = admins.OrderByDescending(a => a.Contact);
                    break;
                case "email":
                    admins = admins.OrderBy(a => a.Email);
                    break;
                case "email_desc":
                    admins = admins.OrderByDescending(a => a.Email);
                    break;
                case "activeUser":
                    admins = admins.OrderBy(a => a.IsActive);
                    break;
                case "activeUser_desc":
                    admins = admins.OrderByDescending(a => a.IsActive);
                    break;
                default:
                    admins = admins.OrderBy(a => a.FirstName);
                    break;
            }
            int pageNumber = (page ?? 1);
            return View(admins.ToPagedList(pageNumber, 5));
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (!string.IsNullOrEmpty(user.Contact) && !string.IsNullOrEmpty(user.Password))
            {
                User userInfo = context.Users
                            .FirstOrDefault(u => u.Contact.Equals(user.Contact) && u.Password.Equals(user.Password));
                return RedirectToAction("Index", "Home", userInfo);
            }
            else
            {
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult RegisterUser() {
            ViewData["action"] = "Register User";
            return View("RegisterUser", new User() { PageMode = "Add User" });
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            //check weather contact exists on DB or not
            if (TempData["okContact"] == null && user.PageMode.Trim().ToLower().Contains("register"))
            {
                string msg = Check.ContactExists(context, user.Contact);
                if (!String.IsNullOrEmpty(msg))
                {
                    ModelState.AddModelError(nameof(user.Contact), msg);
                }
            }

            //check weather email exists on DB or not
            if (TempData["okEmail"] == null && user.PageMode.Trim().ToLower().Contains("register"))
            {
                string msg = Check.EmailExists(context, user.Email);
                if (!String.IsNullOrEmpty(msg))
                {
                    ModelState.AddModelError(nameof(user.Email), msg);
                }
            }

            if (ModelState.IsValid)
            {
                //new dealer added
                if (user.UserId == 0)
                {
                    //add user into DB
                    context.Users.Add(user);
                    context.SaveChanges();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    //Update user into DB
                    context.Users.Update(user);
                    user.DateAdded = DateTime.UtcNow;
                    user.IsActive = true;
                    context.SaveChanges();
                    if (user.LoginType.Trim().ToLower().Contains("user"))
                        return RedirectToAction("Index", "User");
                    else
                        return RedirectToAction("Admin", "User");
                }
            }
            else {
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            ViewData["action"] = "Edit User";
            User user = context.Users.Find(id);
            user.PageMode = "Edit User";
            return View("RegisterUser", user);
        }

        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            var user = context.Users.Find(id);
            user.PageMode = "Delete User";
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteUser(User user)
        {
            if (user.UserId > 0)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
            return RedirectToAction("Index", "User");
        }

    }
}
