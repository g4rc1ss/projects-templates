using System.ComponentModel.DataAnnotations;

namespace CustomArchitecture.API.Models;

public class Request
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }
}
