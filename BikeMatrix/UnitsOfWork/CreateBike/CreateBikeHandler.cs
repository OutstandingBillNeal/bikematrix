using Ardalis.GuardClauses;
using BikeData;
using BikeMatrix.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class CreateBikeHandler(IDbContextFactory<BikesContext> contextFactory)
    : IRequestHandler<CreateBikeRequest, Bike?>
{
    private readonly IDbContextFactory<BikesContext> _contextFactory = contextFactory;

    public async Task<Bike?> Handle(CreateBikeRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_contextFactory);
        Guard.Against.Null(request);
        Guard.Against.Null(request.Bike);

        var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        context.Bikes.Add(request.Bike);
        await context.SaveChangesAsync(cancellationToken);

        return await context.Bikes.FirstOrDefaultAsync(f => f.Id == request.Bike.Id, cancellationToken);

    }
}
