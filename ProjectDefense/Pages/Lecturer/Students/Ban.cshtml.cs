using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Web.Pages.Lecturer.Students
{
    [Authorize(Roles = "Lecturer")]
    public class BanModel(IMediator mediator, UserManager<User> userManager) : PageModel
    {
        public SelectList Students { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Student")]
            public string StudentId { get; init; }

            [Display(Name = "Permanent ban")]
            public bool IsPermanent { get; init; }

            [Range(1, 3650)]
            [Display(Name = "Duration (days)")]
            public int DurationDays { get; init; } = 30;

            [StringLength(500)]
            [Display(Name = "Reason (optional)")]
            public string Reason { get; init; }
        }

        public async Task OnGetAsync()
        {
            await LoadAvailableStudentsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadAvailableStudentsAsync();
                return Page();
            }

            DateTimeOffset? banUntil = Input.IsPermanent
                ? null
                : DateTimeOffset.UtcNow.AddDays(Input.DurationDays);

            try
            {
                await mediator.Send(new BanStudentCommand(
                    Input.StudentId,
                    Input.Reason,
                    banUntil));

                StatusMessage = "Student has been banned successfully.";
                return RedirectToPage("/Lecturer/Index");
            }
            catch (Exception ex)
            {
                await LoadAvailableStudentsAsync(Input.StudentId);
                ModelState.AddModelError(string.Empty, $"Failed to ban student: {ex.Message}");
                return Page();
            }
        }

        private async Task LoadAvailableStudentsAsync(string selectedStudentId = null)
        {
            var allStudents = await userManager.GetUsersInRoleAsync("Student");
            var bannedStudent = await mediator.Send(new GetAllBannedStudentsQuery());

            var availableStudents = allStudents.Where(s => !bannedStudent.Select(b => b.Id).Contains(s.Id)).ToList();

            Students = new SelectList(availableStudents, "Id", "UserName", selectedStudentId);
        }
    }
}