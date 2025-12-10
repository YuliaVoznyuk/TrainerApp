using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Infrastructure.Persistence;

public class AppDbContext: IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Trainer> Trainers { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<TrainingType> TrainingTypes { get; set; } = null!;
    public DbSet<ScheduleSlot> ScheduleSlots { get; set; } = null!;
    public DbSet<WorkoutSession> WorkoutSessions { get; set; } = null!;
    public DbSet<ExerciseRecord> ExerciseRecords { get; set; } = null!;
    public DbSet<NutritionPlan> NutritionPlans { get; set; } = null!;
    public DbSet<NutritionItem> NutritionItems { get; set; } = null!;
    public DbSet<Photo> Photos { get; set; } = null!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One Trainer -> many Clients
            builder.Entity<Client>()
                .HasOne(c => c.Trainer)
                .WithMany(t => t.Clients)
                .HasForeignKey(c => c.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-Many: Client <-> ScheduleSlot
            builder.Entity<ScheduleSlot>()
                .HasMany(s => s.Clients)
                .WithMany(c => c.ScheduleSlots)
                .UsingEntity(j => j.ToTable("ScheduleSlotClients"));

            // Trainer <-> ScheduleSlot (1-to-many)
            builder.Entity<ScheduleSlot>()
                .HasOne(s => s.Trainer)
                .WithMany(t => t.ScheduleSlots)
                .HasForeignKey(s => s.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Trainer <-> TrainingType (many-to-many)
            builder.Entity<Trainer>()
                .HasMany(t => t.TrainingTypes)
                .WithMany(tt => tt.Trainers)
                .UsingEntity(j => j.ToTable("TrainerTrainingTypes"));

            // Client <-> WorkoutSession (1-to-many)
            builder.Entity<WorkoutSession>()
                .HasOne(ws => ws.Client)
                .WithMany(c => c.WorkoutSessions)
                .HasForeignKey(ws => ws.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Trainer <-> WorkoutSession (1-to-many)
            builder.Entity<WorkoutSession>()
                .HasOne(ws => ws.Trainer)
                .WithMany()
                .HasForeignKey(ws => ws.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // NutritionPlan <-> NutritionItem (1-to-many)
            builder.Entity<NutritionPlan>()
                .HasMany(np => np.Items)
                .WithOne(ni => ni.NutritionPlan)
                .HasForeignKey(ni => ni.NutritionPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Trainer <-> NutritionPlan (1-to-many)
            builder.Entity<NutritionPlan>()
                .HasOne(np => np.Trainer)
                .WithMany(t => t.NutritionPlans)
                .HasForeignKey(np => np.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Client <-> NutritionPlan (1-to-many)
            builder.Entity<NutritionPlan>()
                .HasOne(np => np.Client)
                .WithMany(c => c.NutritionPlans)
                .HasForeignKey(np => np.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Photo <-> Trainer / Client
            builder.Entity<Photo>()
                .HasOne(p => p.User)
                .WithMany(u => u.Photos)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Certificate>()
                .HasOne(c => c.Trainer)
                .WithMany(t => t.Certificates)
                .HasForeignKey(c => c.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
