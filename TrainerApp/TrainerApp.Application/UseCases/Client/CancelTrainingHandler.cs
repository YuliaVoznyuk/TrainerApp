using TrainerApp.Application.Interfaces.Repositories;

namespace TrainerApp.Application.UseCases.Client;

public class CancelTrainingHandler
{
    private readonly ITrainingRepository _trainingRepository;

    public CancelTrainingHandler(ITrainingRepository trainingRepository)
    {
        _trainingRepository = trainingRepository;
    }

    public async Task HandleAsync(Guid clientId, Guid slotId)
    {
        await _trainingRepository.RemoveClientFromSlotAsync(clientId, slotId);
    }
}