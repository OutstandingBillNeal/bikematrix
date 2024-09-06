using Microsoft.EntityFrameworkCore;
using UnitsOfWork;
using TestHelpers;
using BikeData;

namespace UnitOfWorkTests;

public class CreateBikeHandlerTests
{
    [Fact]
    public async Task Throws_when_request_is_null()
    {
        // Arrange
        var dbContext = new BikesContext();
        var factory = await MockBikesContextFactory.GetBikesContextFactory(dbContext);
        var sut = new CreateBikeHandler(factory);
        CreateBikeRequest? request = null;

        // Assert
#pragma warning disable CS8604 // Possible null reference argument.
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.Handle(request, CancellationToken.None));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    [Fact]
    public async Task Bike_is_added()
    {
        // Arrange
        var dbContext = new BikesContext();
        var factory = await MockBikesContextFactory.GetBikesContextFactory(dbContext);
        var sut = new CreateBikeHandler(factory);
        var bike = Any.Bike();
        bike.Id = 0;
        var request = new CreateBikeRequest { Bike = bike };
        var numberOfBikesBefore = await dbContext.Bikes.CountAsync(CancellationToken.None);

        // Act
        await sut.Handle(request, CancellationToken.None);

        // Assert
        var numberOfBikesAfter = await dbContext.Bikes.CountAsync(CancellationToken.None);
        Assert.Equal(numberOfBikesBefore + 1, numberOfBikesAfter);

        // Tidy up
        dbContext.Bikes.Remove(bike);
        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

}