using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.API.Models;

public class Request
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }
}
