namespace CoreDrivenArchitecture.API.Models;

public class CreateVehicleRequest
{
    public required string Name { get; set; }
    public required string Make { get; set; }
}