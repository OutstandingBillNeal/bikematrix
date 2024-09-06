using BikeMatrix.Data.Models;
using TestHelpers;
using UnitsOfWork;

namespace UnitOfWorkTests;

public class CreateBikeValidatorTests
{
    [Fact]
    public void Id_must_not_be_non_zero()
    {
        // Arrange
        var bike = Any.Bike();
        Assert.NotEqual(0, bike.Id);
        var request = new CreateBikeRequest { Bike = bike };
        var sut = new CreateBikeValidator();

        // Act
        var result = sut.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        var bikeIdError = result
            .Errors
            .Find(e => e.PropertyName == $"{nameof(Bike)}.{nameof(Bike.Id)}" && e.ErrorMessage.Contains("zero"));
        Assert.NotNull(bikeIdError);
    }

    [Fact]
    public void Valid_bikes_pass_validation()
    {
        // Arrange
        var bike = Any.Bike();
        bike.Id = 0;
        var request = new CreateBikeRequest { Bike = bike };
        var sut = new CreateBikeValidator();

        // Act
        var result = sut.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

}
