using Microsoft.AspNetCore.Identity;
using TrainerApp.Application.DTOs;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.UseCases.Auth;


    public class RegisterUserHandler
    {
        private readonly UserManager<User> _userManager;

        public RegisterUserHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> HandleAsync(RegisterDto dto)
        {
            // 1. Створюємо користувача залежно від ролі
            User user = dto.Role switch
            {
                "Trainer" => new Trainer
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Birthdate = dto.Birthdate
                },
                "Client" => new Domain.Entities.Client
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Birthdate = dto.Birthdate
                },
                _ => throw new ArgumentException("Invalid role.")
            };

            // 2. Реєструємо в Identity
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

            // 3. Додаємо роль
            await _userManager.AddToRoleAsync(user, dto.Role);

            // 4. Повертаємо користувача
            return user;
        }
    }
