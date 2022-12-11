using DealerPeak.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using X.PagedList;

namespace DealerPeak.Controllers
{
    public class VehicleController : Controller
    {
        private DealerPeakDbContext context { get; set; }
        public VehicleController(DealerPeakDbContext dpc)
        {
            this.context = dpc;
        }

        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TypeSortParm = sortOrder == "type" ? "type_desc" : "type";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.MakeYearSortParm = sortOrder == "makeYear" ? "makeYear_desc" : "makeYear";
            ViewBag.ModelSortParm = sortOrder == "model" ? "model_desc" : "model";
            ViewBag.VINSortParm = sortOrder == "vin" ? "vin_desc" : "vin";
            ViewBag.NewVehicleSortParm = sortOrder == "newVehicle" ? "newVehicle_desc" : "newVehicle";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var vehicles = from v in context.Vehicles
                          select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(v => Convert.ToString(v.VehicleName).Contains(searchString)
                                          || v.VehiclePrice.ToString().Contains(searchString)
                                          || Convert.ToString(v.VehicleModel).Contains(searchString)
                                          || Convert.ToString(v.VehicleMakeYear).Contains(searchString)
                                          || Convert.ToString(v.VehicleType).Contains(searchString)
                                          || Convert.ToString(v.Vin.Trim()).Contains(searchString)
                                          || (v.NewVehicle == true ? "New" : "Used").Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vehicles = vehicles.OrderByDescending(v => v.VehicleName);
                    break;
                case "type":
                    vehicles = vehicles.OrderBy(v => v.VehicleType);
                    break;
                case "type_desc":
                    vehicles = vehicles.OrderByDescending(v => v.VehicleType);
                    break;
                case "price":
                    vehicles = vehicles.OrderBy(v => v.VehiclePrice);
                    break;
                case "price_desc":
                    vehicles = vehicles.OrderByDescending(v => v.VehiclePrice);
                    break;
                case "makeYear":
                    vehicles = vehicles.OrderBy(v => v.VehicleMakeYear);
                    break;
                case "makeYear_desc":
                    vehicles = vehicles.OrderByDescending(v => v.VehicleMakeYear);
                    break;
                case "model":
                    vehicles = vehicles.OrderBy(v => v.VehicleModel);
                    break;
                case "model_desc":
                    vehicles = vehicles.OrderByDescending(v => v.VehicleModel);
                    break;
                case "vin":
                    vehicles = vehicles.OrderBy(v => v.Vin);
                    break;
                case "vin_desc":
                    vehicles = vehicles.OrderByDescending(v => v.Vin);
                    break;
                case "newVehicle":
                    vehicles = vehicles.OrderBy(v => v.NewVehicle);
                    break;
                case "newVehicle_desc":
                    vehicles = vehicles.OrderByDescending(v => v.NewVehicle);
                    break;
                default:
                    vehicles = vehicles.OrderBy(v => v.VehicleName);
                    break;
            }
            int pageNumber = (page ?? 1);
            return View(vehicles.ToPagedList(pageNumber, 5));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddVehicle()
        {
            ViewData["action"] = "AddVehicle";
            return View("EditVehicle", new Vehicle() { PageMode = "Add Vehicle" });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditVehicle(int id)
        {
            ViewData["action"] = "EditVehicle";
            var vehicle = context.Vehicles.Find(id);
            vehicle.PageMode = "Edit Vehicle";
            return View(vehicle);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditVehicle(Vehicle vehicle)
        {
            //check weather VIN exists on DB or not
            if (TempData["okVIN"] == null)
            {
                string msg = Check.VINExists(context, vehicle.Vin.ToString());
                if (!String.IsNullOrEmpty(msg) && vehicle.PageMode.Trim().ToLower().Contains("add"))
                {
                    ModelState.AddModelError(nameof(vehicle.Vin), msg);
                }
            }

            if (ModelState.IsValid)
            {
                //new vehicle added
                if (vehicle.VehicleId == 0)
                {
                    context.Vehicles.Add(vehicle);
                    context.SaveChanges();
                }
                else
                {
                    //updating an existing vehicle
                    vehicle.DateAdded = DateTime.UtcNow;
                    context.Vehicles.Update(vehicle);
                    context.SaveChanges();
                }
                return RedirectToAction("Index", "Vehicle");
            }
            else
            {
                return RedirectToAction("AddVehicle", "Vehicle");
            }
        }

        [HttpGet]
        [Route("vehicles/search/{min}/{max}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Search(decimal min, decimal max)
        {
            var vehicles = context.Vehicles
                                  .Where(v => v.VehiclePrice >= min && v.VehiclePrice <= max);
            return new JsonResult(vehicles);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteVehicle(int id)
        {
            var vehicle = context.Vehicles.Find(id);
            return View(vehicle);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteVehicle(Vehicle vehicle)
        {
            if (vehicle.VehicleId > 0)
            {
                context.Vehicles.Remove(vehicle);
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Vehicle");
        }
    }
}
