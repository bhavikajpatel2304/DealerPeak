using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealerPeak.Models;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    [Required(ErrorMessage = "Please select Vehicle Type.")]
    public string? VehicleType { get; set; }

    [Required(ErrorMessage = "Please enter a Vehicle Name.")]
    public string? VehicleName { get; set; }

    [Required(ErrorMessage = "Please enter a Vehicle Model.")]
    public string? VehicleModel { get; set; }

    [Required(ErrorMessage = "Please enter a Vehicle Make Year.")]
    [RegularExpression("^([0-9]{4})$", ErrorMessage = "Please Enter valid Make Year.")]
    public string? VehicleMakeYear { get; set; }

    [Required(ErrorMessage = "Please enter a Vehicle Price.")]
    [RegularExpression("^[0-9]+(\\.[0-9]{1,2})$", ErrorMessage = "Please enter price with maximum 2 decimal places.")]
    public decimal? VehiclePrice { get; set; }

    [Required(ErrorMessage = "Please select Vehicle Age as New or Old.")]
    public bool? NewVehicle { get; set; }

    [Required(ErrorMessage = "Please enter a VIN.")]
    [RegularExpression("^([a-zA-Z0-9]{17})$", ErrorMessage = "Please Enter valid VIN.")]
    [Remote("CheckVIN", "Validation", AdditionalFields = nameof(PageMode))]
    public string? Vin { get; set; }

    //to restrict remote validation on Edit mode
    [NotMapped]
    public string PageMode { get; set; } 

    public DateTime? DateAdded { get; set; }

    public virtual ICollection<DealerVehicle> DealerVehicles { get; } = new List<DealerVehicle>();
}
