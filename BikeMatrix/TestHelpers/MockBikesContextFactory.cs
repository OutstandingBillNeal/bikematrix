using BikeData;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestHelpers
{
    public class MockBikesContextFactory
    {
        protected MockBikesContextFactory() { }

        public async static Task<IDbContextFactory<BikesContext>> GetBikesContextFactory(BikesContext context)
        {
            var factory = new Mock<IDbContextFactory<BikesContext>>();
            factory
                .Setup(m => m.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(context);
            return await Task.FromResult(factory.Object);
        }
    }
}
