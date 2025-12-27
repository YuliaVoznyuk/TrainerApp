using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces.Repositories;

namespace TrainerApp.Application.UseCases.Client;

public class GetClientTrainingsHandler
{
    private readonly ITrainingRepository _trainingRepository;

    public GetClientTrainingsHandler(ITrainingRepository trainingRepository)
    {
        _trainingRepository = trainingRepository;
    }

    public async Task<IEnumerable<TrainingPlan>> HandleAsync(Guid clientId)
    {
        var slots = await _trainingRepository.GetClientTrainingsAsync(clientId);
        return slots.Select(s => new TrainingPlan
        {
            SlotId = s.Id,
            StartAt = s.StartAt,
            EndAt = s.EndAt,
            TrainerName = $"{s.Trainer.FirstName} {s.Trainer.LastName}"
        });
    }
}