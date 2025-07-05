using System.ComponentModel.DataAnnotations;

namespace MvcArchitecture.API.Models;

public class Request
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }
}
