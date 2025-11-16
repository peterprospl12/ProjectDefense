using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Application.UseCases.Queries;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Web.Pages.Lecturer.Reservations
{
    [Authorize(Roles = "Lecturer")]
    public class IndexModel(IMediator mediator, UserManager<User> userManager) : PageModel
    {
        public IEnumerable<ReservationDto> Reservations { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            var lecturer = await userManager.GetUserAsync(User);
            Reservations = await mediator.Send(new GetAllReservationsQuery(lecturer.Id));
        }

        public async Task<IActionResult> OnPostCancelAsync(int reservationId)
        {
            var lecturer = await userManager.GetUserAsync(User);
            try
            {
                await mediator.Send(new CancelReservationCommand
                (
                    reservationId,
                    lecturer.Id
                ));
                StatusMessage = "Reservation has been successfully cancelled.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
