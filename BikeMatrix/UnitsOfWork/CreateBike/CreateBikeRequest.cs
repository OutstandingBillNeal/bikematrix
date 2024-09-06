using BikeMatrix.Data.Models;
using MediatR;

namespace UnitsOfWork;

public class CreateBikeRequest
    : IRequest<Bike?>
{
    public required Bike Bike { get; set; }
}
