using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Domain.Entities;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Web.Pages.Student
{
    [Authorize(Roles = "Student")]
    public class MyReservationsModel(IMediator mediator, UserManager<User> userManager) : PageModel
    {
        public IEnumerable<ReservationDto> MyReservations { get; set; } = [];
        public IEnumerable<AvailableReservationDto> AvailableSlots { get; set; } = [];

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            var student = await userManager.GetUserAsync(User);
            if (student == null) return;

            MyReservations = await mediator.Send(new GetUserReservationsQuery(student.Id));

            var slots = await mediator.Send(new GetAvailableSlotsQuery(null));
            AvailableSlots = slots
                .Where(s => MyReservations.All(m => m.Id != s.Id))
                .OrderBy(s => s.StartTime).ToList();
        }

        private async Task<bool> IsStudentBanned(string studentId)
        {
            if (await mediator.Send(new GetStudentBlockStatusQuery(studentId)))
            {
                StatusMessage = "Error: Your account is blocked. You cannot manage reservations.";
                return true;
            }
            return false;
        }

        public async Task<IActionResult> OnPostCancelAsync(int reservationId)
        {
            var student = await userManager.GetUserAsync(User);
            if (student == null) return Forbid();
            if (await IsStudentBanned(student.Id)) return RedirectToPage();

            try
            {
                await mediator.Send(new CancelReservationCommand(reservationId, student.Id));
                StatusMessage = "Reservation cancelled.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangeAsync(int currentReservationId, int newReservationId)
        {
            var student = await userManager.GetUserAsync(User);
            if (student == null) return Forbid();
            if (await IsStudentBanned(student.Id)) return RedirectToPage();

            try
            {
                await mediator.Send(new ChangeReservationCommand(currentReservationId, newReservationId, student.Id));
                StatusMessage = "Reservation changed successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}