using BikeData;
using TestHelpers;

namespace BikeMatrix.DataTests
{
    public class DbContextTests
    {
        [Fact]
        public void Bikes_can_be_saved()
        {
            // Arrange
            var bikeModel = Guid.NewGuid().ToString();
            var sut = new BikesContext();
            var bike = Any.Bike();
            bike.Id = 0;
            bike.Model = bikeModel;

            // Act
            sut.Bikes.Add(bike);
            sut.SaveChanges();

            // Assert
            var retrievedBike = sut.Bikes.FirstOrDefault(f => f.Model == bikeModel);
            Assert.NotNull(retrievedBike);
        }

        [Fact]
        public void Saving_a_Bike_assigns_an_id()
        {
            // Arrange
            var sut = new BikesContext();
            var bike = Any.Bike();
            bike.Id = 0;
            sut.Bikes.Add(bike);

            // Act
            sut.SaveChanges();

            // Assert
            Assert.NotEqual(0, bike.Id);
        }

        [Fact]
        public void Bikes_can_be_deleted()
        {
            // Arrange
            var sut = new BikesContext();
            var bike = Any.Bike();
            bike.Id = 0;
            sut.Bikes.Add(bike);
            sut.SaveChanges();
            var bikeId = bike.Id;

            // Act
            sut.Bikes.Remove(bike);
            sut.SaveChanges();

            // Assert
            var bikeRetrievedAfterDeletion = sut.Bikes.FirstOrDefault(f => f.Id == bikeId);
            Assert.Null(bikeRetrievedAfterDeletion);
        }

        [Fact]
        public void Bikes_can_be_updated()
        {
            // Arrange
            var sut = new BikesContext();
            var bike = Any.Bike();
            var initialBikeModel = Guid.NewGuid().ToString();
            bike.Model = initialBikeModel;
            sut.Bikes.Add(bike);
            sut.SaveChanges();
            var bikeId = bike.Id;

            // Act
            var updatedBikeModel = Guid.NewGuid().ToString();
            bike.Model = updatedBikeModel;
            sut.SaveChanges();

            // Assert
            var bikeRetrievedAfterUpdate = sut.Bikes.First(f => f.Id == bikeId);
            Assert.Equal(updatedBikeModel, bikeRetrievedAfterUpdate.Model);
        }

    }
}