using BikeMatrix.Data.Models;
using BikeMatrix.Server.Controllers;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestHelpers;
using UnitsOfWork;

namespace BikesApiTests;

public class BikesControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<IValidator<CreateBikeRequest>> _createBikeValidator = new();

    [Fact]
    public async Task Get_returns_values_with_200()
    {
        // Arrange
        var bike1 = Any.Bike();
        var bike2 = Any.Bike();
        var mediatorResult = new[] { bike1, bike2 };
        _mediator
            .Setup(m => m.Send(It.IsAny<GetBikesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        // Act
        var result = await Sut.Get();

        // Assert general cleanliness
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ActionResult<IEnumerable<Bike>>>(result);
        Assert.NotNull(result.Result);
        // Assert 200 - Ok
        Assert.IsType<OkObjectResult>(result.Result);
        var okObjectResult = (OkObjectResult)result.Result;
        Assert.NotNull(okObjectResult);
        Assert.NotNull(okObjectResult.Value);
        Assert.IsAssignableFrom<IEnumerable<Bike>>(okObjectResult.Value);
        var returnedBikes = (IEnumerable<Bike>)okObjectResult.Value;
        var returnedBike1 = returnedBikes.FirstOrDefault(f => f.Model == bike1.Model);
        var returnedBike2 = returnedBikes.FirstOrDefault(f => f.Model == bike2.Model);
        Assert.NotNull(returnedBike1);
        Assert.NotNull(returnedBike2);
        // Assert correct data returned
        Assert.Equal(bike1, returnedBike1);
        Assert.Equal(bike2, returnedBike2);
    }

    [Fact]
    public async Task Post_valid_bike_returns_value_and_location_with_201()
    {
        // Arrange
        var bike1 = Any.Bike();
        _mediator
            .Setup(m => m.Send(It.IsAny<CreateBikeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bike1);
        var validResult = new ValidationResult();
        Assert.True(validResult.IsValid); // Show the reader the implied validity.
        _createBikeValidator
            .Setup(m => m.ValidateAsync(It.IsAny<CreateBikeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validResult);

        // Act
        var result = await Sut.Post(bike1);

        // Some people debate the "correctness" of asserting multiple things in a single test.
        // The argument is along the lines of "One test - one reason for failure.  A test
        // failure can point directly to the problem without further investigation."
        // While that argument has some merit, if one assumes that the test will pass more
        // often than it will fail, then the relative importance of legibility increases.
        // A less verbose, less repetetive test class is more legible than a more verbose one.
        // The downside of multiple assertions in a test is that it requires some effort to
        // determine which assertion failed.  However, this is usually quite a simple task.

        // Assert general cleanliness
        Assert.NotNull(result);
        // Assert 201 - Created
        Assert.IsType<CreatedAtActionResult>(result);
        var createdResult = (CreatedAtActionResult)result;
        Assert.NotNull(createdResult);
        Assert.NotNull(createdResult.Value);
        Assert.IsAssignableFrom<Bike>(createdResult.Value);
        var returnedBike = (Bike)createdResult.Value;
        Assert.NotNull(returnedBike);
        // Assert correct data returned
        Assert.Equal(bike1, returnedBike);
        // Assert correct location returned
        Assert.Equal(nameof(BikesController.GetById), createdResult.ActionName);
        Assert.NotNull(createdResult.RouteValues);
        // ... and by the time you get down here, where the value to be tested requires several
        // steps of derivation, it becomes less practcal to put each assertion into its own test.
        Assert.Equal(returnedBike.Id, createdResult.RouteValues["id"]);
    }

    [Fact]
    public async Task Post_invalid_bike_returns_400()
    {
        // Arrange
        var bike1 = Any.Bike();
        _mediator
            .Setup(m => m.Send(It.IsAny<CreateBikeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bike1);
        var invalidResult = new ValidationResult([new ValidationFailure()]);
        Assert.False(invalidResult.IsValid); // Show the reader the implied validity.
        _createBikeValidator
            .Setup(m => m.ValidateAsync(It.IsAny<CreateBikeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(invalidResult);

        // Act
        var result = await Sut.Post(bike1);

        // Assert general cleanliness
        Assert.NotNull(result);
        // Assert 400 - Bad request
        Assert.IsType<BadRequestObjectResult>(result);
    }

    private BikesController Sut
        => new(_mediator.Object, _createBikeValidator.Object);

}
