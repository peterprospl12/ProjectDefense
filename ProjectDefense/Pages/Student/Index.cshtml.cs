using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Application.UseCases.Queries;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Web.Pages.Student
{
    [Authorize(Roles = "Student")]
    public class IndexModel(IMediator mediator, UserManager<User> userManager) : PageModel
    {
        public IEnumerable<AvailableReservationDto> AvailableSlots { get; set; } = [];
        public bool HasActiveBooking { get; set; }
        public int ActiveBookingsCount { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            var student = await userManager.GetUserAsync(User);
            if (student == null) return;

            var myReservations = await mediator.Send(new GetUserReservationsQuery(student.Id));

            HasActiveBooking = myReservations.Any();
            ActiveBookingsCount = myReservations.Count();

            var slots = await mediator.Send(new GetAvailableSlotsQuery(null));
            AvailableSlots = slots.OrderBy(s => s.StartTime).ToList();
        }

        public async Task<IActionResult> OnPostBookAsync(int reservationId)
        {
            var student = await userManager.GetUserAsync(User);
            if (student == null) return Forbid();

            var myReservations = await mediator.Send(new GetUserReservationsQuery(student.Id));

            var hasAlreadyBooked = myReservations.Any();

            if (hasAlreadyBooked)
            {
                StatusMessage = "You currently have an active reservation. Please cancel it before booking a new slot.";
                return RedirectToPage();
            }

            try
            {
                await mediator.Send(new BookReservationCommand
                (
                    reservationId,
                    student.Id
                ));

                return RedirectToPage("/Student/MyReservations");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                return RedirectToPage();
            }
        }
    }
}