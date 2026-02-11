using Mapster;
using TrainerApp.Application.DTOs;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Mapping;

public class NutritionMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // DTO -> Entity
        config.NewConfig<CreateNutritionPlanDto, NutritionPlan>()
            .Ignore(d => d.Id)
            .Ignore(d => d.TrainerId)
            .Ignore(d => d.StartDate)
            .Ignore(d => d.Items);

        config.NewConfig<CreateNutritionItemDto, NutritionItem>()
            .Ignore(d => d.Id);

        // Entity -> DTO
        config.NewConfig<NutritionPlan, NutritionPlanDto>();
        config.NewConfig<NutritionItem, NutritionItemDto>();
    }
}