using DealerPeak.Models;
using Microsoft.AspNetCore.Mvc;

namespace DealerPeak.Controllers
{
    public class ValidationController : Controller
    {
        private DealerPeakDbContext context;
        public ValidationController(DealerPeakDbContext dpc) => context = dpc;
        public JsonResult CheckEmail(string emailAddress, string PageMode)
        {
            if (PageMode.Trim().ToLower().Contains("add"))
            {
                string msg = Check.EmailExists(context, emailAddress);
                if (string.IsNullOrEmpty(msg))
                {
                    TempData["okEmail"] = true;
                    return Json(true);
                }
                else return Json(msg);
            }
            else return Json(true);
        }

        public JsonResult CheckContact(string contact, string PageMode)
        {
            if (PageMode.Trim().ToLower().Contains("add"))
            {
                string msg = Check.ContactExists(context, contact);
                if (string.IsNullOrEmpty(msg))
                {
                    TempData["okContact"] = true;
                    return Json(true);
                }
                else return Json(msg);
            }
            else return Json(true);
        }

        public JsonResult CheckVIN(string VIN, string PageMode)
        {
            if (PageMode.Trim().ToLower().Contains("add"))
            {
                string msg = Check.VINExists(context, VIN);
                if (string.IsNullOrEmpty(msg))
                {
                    TempData["okVIN"] = true;
                    return Json(true);
                }
                else return Json(msg);
            }
            else return Json(true);
        }
    }
}
