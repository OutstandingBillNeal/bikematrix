using BikeMatrix.Data.Models;
using MediatR;

namespace UnitsOfWork;

public class GetBikesRequest
    : IRequest<IEnumerable<Bike>>
{
}