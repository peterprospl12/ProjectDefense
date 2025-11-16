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
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public IndexModel(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public IEnumerable<ReservationDto> Reservations { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            var lecturer = await _userManager.GetUserAsync(User);
            Reservations = await _mediator.Send(new GetAllReservationsQuery(lecturer.Id));
        }

        public async Task<IActionResult> OnPostCancelAsync(int reservationId)
        {
            var lecturer = await _userManager.GetUserAsync(User);
            try
            {
                await _mediator.Send(new CancelReservationCommand
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
