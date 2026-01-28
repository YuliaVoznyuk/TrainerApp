using System.Security.Claims;
using TrainerApp.Application.Interfaces;

namespace TrainerApp.API.Context;

public class TrainerContext : ITrainerContext
{
    public Guid TrainerId { get; }

    public TrainerContext(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User
                   ?? throw new UnauthorizedAccessException();

        var trainerIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? throw new UnauthorizedAccessException("TrainerId claim is missing");

        TrainerId = Guid.Parse(trainerIdClaim);
    }
}
