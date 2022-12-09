using DealerPeak.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using X.PagedList;

namespace DealerPeak.Controllers
{
    public class DealerVehicleController : Controller
    {
        private DealerPeakDbContext context { get; set; }
        public DealerVehicleController(DealerPeakDbContext dpc)
        {
            this.context = dpc;
        }

        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.VehicleIdSortParm = String.IsNullOrEmpty(sortOrder) ? "vehicle_desc" : "";
            ViewBag.DealerIdSortParm = sortOrder == "dealer" ? "dealer_desc" : "dealer";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //var dealerVehicles = from dv in context.DealerVehicles
            //                     join v in context.Vehicles
            //                     on dv.VehicleId equals v.VehicleId
            //                     join d in context.Dealers
            //                     on dv.DealerId equals d.DealerId
            //                     select new { 
            //                        VehicleId = dv.VehicleId,
            //                        DealerId = dv.DealerId,
            //                        VehicleName = v.VehicleName,
            //                        VIN = v.Vin,
            //                        DealerName = d.DealerName
            //                     };


            var dealerVehicles = from dv in context.DealerVehicles
                                 select dv;

            if (!String.IsNullOrEmpty(searchString))
            {
                dealerVehicles = dealerVehicles.Where(dv => Convert.ToString(dv.VehicleId).Contains(searchString)
                                                         || Convert.ToString(dv.DealerId).Contains(searchString));
            }

            switch (sortOrder)
            {
                case "vehicle_desc":
                    dealerVehicles = dealerVehicles.OrderByDescending(dv => dv.VehicleId);
                    break;
                case "dealer":
                    dealerVehicles = dealerVehicles.OrderBy(dv => dv.DealerId);
                    break;
                case "dealer_desc":
                    dealerVehicles = dealerVehicles.OrderByDescending(dv => dv.DealerId);
                    break;
                default:
                    dealerVehicles = dealerVehicles.OrderBy(dv => dv.DealerId);
                    break;
            }
            //int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(dealerVehicles.ToPagedList(pageNumber, 5));
        }

        public List<SelectListItem> VehicleList()
        {
            List<SelectListItem> lstVehicles = new List<SelectListItem>();
            var vehicleList = from v in context.Vehicles
                              select new { VehicleId = v.VehicleId, VehicleName = v.VehicleName };

            foreach (var vehicle in vehicleList) {
                lstVehicles.Add(new SelectListItem
                {
                    Value = vehicle.VehicleId.ToString(),
                    Text = vehicle.VehicleName.ToString()
                });
            };

            return lstVehicles;
        }

        public List<SelectListItem> DealerList()
        {
            List<SelectListItem> lstDealers = new List<SelectListItem>();
            var dealerList = from d in context.Dealers
                              select new { DealerId = d.DealerId, DealerName = d.DealerName };

            foreach (var dealer in dealerList)
            {
                lstDealers.Add(new SelectListItem
                {
                    Value = dealer.DealerId.ToString(),
                    Text = dealer.DealerName.ToString()
                });
            };

            return lstDealers;
        }

        [HttpGet]
        public IActionResult MapVehicle()
        {
            TempData["SelectVehicles"] = VehicleList();
            TempData["SelectDealers"] = DealerList();
            ViewData["action"] = "MapVehicle";

            return View();
        }

        [HttpPost]
        [Route("map/{vehicleId}/{dealerId}")]
        public IActionResult Map(int vehicleId, int dealerId)
        {
            //dealer vehicle mapped
            if (vehicleId > 0 && dealerId > 0)
            {
                var count = context.DealerVehicles.OrderByDescending(dv => dv.VehicleId   )
                                   .FirstOrDefault().DealerVehicleId;
                DealerVehicle dv = new DealerVehicle();

                if (count == null || count == 0 )
                    dv.DealerVehicleId = 0;
                else
                    dv.DealerVehicleId = count + 1;
                
                dv.VehicleId = vehicleId;
                dv.DealerId = dealerId;
                dv.DateAdded = DateTime.UtcNow;
                context.DealerVehicles.Add(dv);
                context.SaveChanges();
                return RedirectToAction("Index", "DealerVehicle");

            } else
                return View();

        }

        [HttpGet]
        public IActionResult UnMapVehicle(int id)
        {
            var dv = context.DealerVehicles.Find(id);
            ViewData["DealerVehicleId"] = id;
            return View(dv);
        }

        [HttpPost]
        [Route("unmap/{dealervehicleId}")]
        public IActionResult UnMapVehicle1(int dealervehicleId)
        {
            if (dealervehicleId > 0)
            {
                DealerVehicle dv = new DealerVehicle();
                dv.DealerVehicleId = dealervehicleId;
                context.DealerVehicles.Remove(dv);
                context.SaveChanges();
            }
            return RedirectToAction("Index", "DealerVehicle");
        }
    }
}
