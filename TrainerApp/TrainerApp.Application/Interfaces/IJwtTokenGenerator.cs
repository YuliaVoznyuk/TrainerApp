using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IList<string> roles);

}