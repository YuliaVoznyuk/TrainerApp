using System.ComponentModel.DataAnnotations;

namespace TrainerApp.Domain.Entities;

public class Certificate
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = default!; // Наприклад: "Certified Fitness Trainer"
    [MaxLength(100)]
    public string? Category { get; set; }          // Наприклад: "Nutrition", "Strength Training"
    public DateTime? DateReceived { get; set; }    // Дата отримання сертифікату
    public string? PhotoUrl { get; set; }          // Посилання на фото або pdf сертифікату

    // 🔹 Зв’язок із тренером
    public Guid TrainerId { get; set; }
    public Trainer Trainer { get; set; } = default!;
}