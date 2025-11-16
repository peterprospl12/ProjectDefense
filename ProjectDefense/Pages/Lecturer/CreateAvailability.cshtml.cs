using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Application.UseCases.Queries;
using ProjectDefense.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectDefense.Web.Pages.Lecturer
{
    [Authorize(Roles = "Lecturer")]
    public class CreateAvailabilityModel(IMediator mediator, UserManager<User> userManager) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList Rooms { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Room")]
            public int RoomId { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Start Date")]
            public DateOnly StartDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "End Date")]
            public DateOnly EndDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

            [Required]
            [DataType(DataType.Time)]
            [Display(Name = "Start Time")]
            public TimeOnly StartTime { get; init; }

            [Required]
            [DataType(DataType.Time)]
            [Display(Name = "End Time")]
            public TimeOnly EndTime { get; init; }

            [Required]
            [Range(5, 120, ErrorMessage = "Slot duration must be between 5 and 120 minutes.")]
            [Display(Name = "Slot Duration (minutes)")]
            public int SlotDurationInMinutes { get; init; } = 15;
        }

        public async Task OnGetAsync()
        {
            var rooms = await mediator.Send(new GetAllRoomsQuery());
            Rooms = new SelectList(rooms, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); 
                return Page();
            }

            var lecturer = await userManager.GetUserAsync(User);
            if (lecturer == null)
            {
                return Forbid();
            }

            try
            {
                var command = new CreateAvailabilityCommand(
                    lecturer.Id,
                    Input.RoomId,
                    Input.StartDate,
                    Input.EndDate,
                    Input.StartTime,
                    Input.EndTime,
                    Input.SlotDurationInMinutes
                );

                await mediator.Send(command);

                TempData["StatusMessage"] = "Availability has been successfully created.";
                return RedirectToPage("/Lecturer/Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await OnGetAsync(); 
                return Page();
            }
        }
    }
}