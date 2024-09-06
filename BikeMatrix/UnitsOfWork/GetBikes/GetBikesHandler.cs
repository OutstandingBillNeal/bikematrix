using Ardalis.GuardClauses;
using BikeData;
using BikeMatrix.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class GetBikesHandler(IDbContextFactory<BikesContext> contextFactory)
    : IRequestHandler<GetBikesRequest, IEnumerable<Bike>>
{
    private readonly IDbContextFactory<BikesContext> _contextFactory = contextFactory;

    public async Task<IEnumerable<Bike>> Handle(GetBikesRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_contextFactory);
        Guard.Against.Null(request);

        var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Bikes.ToListAsync(cancellationToken);
    }
}
