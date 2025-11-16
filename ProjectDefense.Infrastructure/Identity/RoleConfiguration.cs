using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProjectDefense.Domain.Entities;
using ProjectDefense.Domain.Enums;

namespace ProjectDefense.Infrastructure.Identity
{
    public static class RoleConfiguration
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = [nameof(Role.Lecturer), nameof(Role.Student)];

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedLecturerAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            const string lecturerEmail = "lecturer@example.com";
            const string lecturerPassword = "Password123!";

            if (await userManager.FindByEmailAsync(lecturerEmail) == null)
            {
                var lecturer = new User
                {
                    UserName = lecturerEmail,
                    Email = lecturerEmail,
                    EmailConfirmed = true,
                    Role = Role.Lecturer
                    
                };

                var result = await userManager.CreateAsync(lecturer, lecturerPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(lecturer, nameof(Role.Lecturer));
                }
            }
        }

        public static async Task SeedStudentsAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            List<string> studentEmails = ["student1@example.com", "student2@example.com"]; 
            const string studentPassword = "Password123!";

            foreach (var studentEmail in studentEmails)
            {
                if (await userManager.FindByEmailAsync(studentEmail) == null)
                {
                    var student = new User
                    {
                        UserName = studentEmail,
                        Email = studentEmail,
                        EmailConfirmed = true,
                        Role = Role.Student

                    };

                    var result = await userManager.CreateAsync(student, studentPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(student, nameof(Role.Student));
                    }
                }
            }
        }
    }
}
