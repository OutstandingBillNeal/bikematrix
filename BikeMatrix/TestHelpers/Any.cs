using BikeMatrix.Data.Models;

namespace TestHelpers
{
    public static class Any
    {
        public static string String()
        {
            return Guid.NewGuid().ToString()[..8];
        }

        public static int Int()
        {
            return new Random().Next(1, 1000000);
        }

        public static int IntBetween(int least, int most)
        {
            return new Random().Next(least, most);
        }

        public static Bike Bike()
        {
            return new Bike
            {
                Id = Any.Int(),
                Brand = Any.String(),
                Model = Any.String(),
                Year = Any.IntBetween(1990, DateTime.Now.Year),
                OwnerEmail = $"{Any.String()}@{Any.String()}.com"
            };
        }
    }
}
