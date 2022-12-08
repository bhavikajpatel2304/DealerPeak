using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DealerPeak.Models;

public partial class User
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Please enter a First Name.")]
    [RegularExpression("(?i)^[a-z0-9 ]+$",
     ErrorMessage = "First name should not contain any special characters.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Please enter a Last Name.")]
    [RegularExpression("(?i)^[a-z0-9 ]+$",
     ErrorMessage = "Last name should not contain any special characters.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Please enter an address.")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Please select a city.")]
    public string? City { get; set; }

    [Required(ErrorMessage = "Please enter a valid Contact.")]
    [RegularExpression("^([0-9]{10})$", ErrorMessage = "Please Enter valid Mobile Number.")]
    [Remote("CheckContact", "Validation", AdditionalFields = nameof(PageMode))]
    public string Contact { get; set; } = null!;

    [Required(ErrorMessage = "Please enter an Email.")]
    [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$",
    ErrorMessage = "Please enter a valid Email Address.")]
    [DataType(DataType.EmailAddress)]
    [Remote("CheckEmail", "Validation", AdditionalFields = nameof(PageMode))]
    public string Email { get; set; } = null!;

    //to restrict remote validation on Edit mode
    [NotMapped]
    public string PageMode { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? DateAdded { get; set; }

    [Required(ErrorMessage = "Please select any one Login Type.")]
    public string LoginType { get; set; } = null!;

    [Required(ErrorMessage = "Please enter password.")]
    [Compare("ConfirmPassword")]
    [StringLength(8, ErrorMessage = "Please limit your password to 8 characters.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Please confirm your password.")]
    [Display(Name = "Confirm Password")]
    [NotMapped]
    public string ConfirmPassword { get; set; }
}
