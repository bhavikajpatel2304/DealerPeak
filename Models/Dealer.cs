using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DealerPeak.Models;

public partial class Dealer
{
    public int DealerId { get; set; }

    [Required(ErrorMessage = "Please enter a Dealer Name.")]
    [RegularExpression("(?i)^[a-z0-9 ]+$",
     ErrorMessage = "Dealer name should not contain any special characters.")]
    public string? DealerName { get; set; }

    [Required(ErrorMessage = "Please enter a Dealer Contact.")]
    [RegularExpression("^([0-9]{10})$", ErrorMessage = "Please Enter valid Mobile Number.")]
    [Remote("CheckContact", "Validation")]
    public string? DealerContact { get; set; }

    [Required(ErrorMessage = "Please enter a Dealer Email.")]
    [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$",
     ErrorMessage = "Please enter a valid Email Address.")]
    [DataType(DataType.EmailAddress)]
    [Remote("CheckEmail", "Validation")]
    public string? DealerEmail { get; set; }

    public DateTime? DateAdded { get; set; }

    public virtual ICollection<DealerVehicle> DealerVehicles { get; } = new List<DealerVehicle>();
}
