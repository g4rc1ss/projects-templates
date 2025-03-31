using CoreDrivenArchitecture.Data.Entities;
using CoreDrivenArchitecture.Data.Repositories;
using CoreDrivenArchitecture.DTOs.Vehicles;
using CoreDrivenArchitecture.UseCases.Vehicles;
using Moq;
using ROP;

namespace CoreDrivenArchitecture.UnitTests.UseCases.Vehicles;

public class AddVehicleTests
{
    private class TestState
    {
        public Mock<IDatabaseRepository> DatabaseRepository { get; set; }
        public AddVehicle Subject { get; set; }

        public TestState()
        {
            DatabaseRepository = new Mock<IDatabaseRepository>();
            Subject = new AddVehicle(DatabaseRepository.Object);
        }
    }

    [Fact]
    public async Task WhenVehicleRequestHasCorrectData_thenInserted()
    {
        TestState state = new();
        string make = "opel";
        string name = "vehicle1";
        int id = 1;
        state.DatabaseRepository.Setup(x => x
                .AddVehicle(It.IsAny<CreateVehicle>()))
            .ReturnsAsync(new VehicleEntity() { Id = id, Make = make, Name = name });


        Result<VehicleDto> result = await state.Subject
            .Execute(new CreateVehicle() { Make = make, Name = name });

        Assert.True(result.Success);
        Assert.Equal(make, result.Value.Make);
        Assert.Equal(id, result.Value.Id);
        Assert.Equal(name, result.Value.Name);
    }
}