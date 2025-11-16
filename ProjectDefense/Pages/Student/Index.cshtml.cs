using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Domain.Entities;
using ProjectDefense.Application.Interfaces;

namespace ProjectDefense.Web.Pages.Student
{
    [Authorize(Roles = "Student")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly IReservationRepository _reservationRepository;

        public IndexModel(IMediator mediator, UserManager<User> userManager, IReservationRepository reservationRepository)
        {
            _mediator = mediator;
            _userManager = userManager;
            _reservationRepository = reservationRepository;
        }

        public IEnumerable<ReservationDto> AvailableSlots { get; set; } = [];
        public bool HasActiveBooking { get; set; }
        public int ActiveBookingsCount { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            var student = await _userManager.GetUserAsync(User);
            if (student == null) return;

            HasActiveBooking = await _reservationRepository.HasStudentAlreadyBookedAsync(student.Id);
            var myReservations = await _reservationRepository.GetActiveReservationsByStudentIdAsync(student.Id);
            ActiveBookingsCount = myReservations.Count();

            var slots = await _reservationRepository.GetAvailableSlotsAsync(null);
           
            AvailableSlots = slots.Select(r => new ReservationDto
            {
                Id = r.Id,
                RoomName = r.Availability?.Room?.Name ?? "-",
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                StudentName = r.Student?.UserName,
            }).OrderBy(s => s.StartTime).ToList();
        }

        public async Task<IActionResult> OnPostBookAsync(int reservationId)
        {
            var student = await _userManager.GetUserAsync(User);
            if (student == null) return Forbid();

            if (await _reservationRepository.HasStudentAlreadyBookedAsync(student.Id))
            {
                StatusMessage = "You currently have an active reservation. Please cancel it before booking a new slot.";
                return RedirectToPage();
            }

            try
            {
                await _mediator.Send(new BookReservationCommand
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