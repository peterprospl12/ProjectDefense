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
    public class MyReservationsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly IReservationRepository _reservationRepository;
        private readonly IStudentBlockRepository _studentBlockRepository;

        public MyReservationsModel(IMediator mediator, UserManager<User> userManager, IReservationRepository reservationRepository, IStudentBlockRepository studentBlockRepository)
        {
            _mediator = mediator;
            _userManager = userManager;
            _reservationRepository = reservationRepository;
            _studentBlockRepository = studentBlockRepository;
        }

        public IEnumerable<ReservationDto> MyReservations { get; set; } = [];
        public IEnumerable<ReservationDto> AvailableSlots { get; set; } = [];

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            var student = await _userManager.GetUserAsync(User);
            if (student == null) return;

            var my = await _reservationRepository.GetActiveReservationsByStudentIdAsync(student.Id);
            MyReservations = my.Select(r => new ReservationDto
            {
                Id = r.Id,
                RoomName = r.Availability?.Room?.Name ?? "-",
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                StudentName = r.Student?.UserName
            }).OrderBy(r => r.StartTime).ToList();

            var slots = await _reservationRepository.GetAvailableSlotsAsync(null);
            AvailableSlots = slots
                .Where(s => !my.Any(m => m.Id == s.Id))
                .Select(r => new ReservationDto
                {
                    Id = r.Id,
                    RoomName = r.Availability?.Room?.Name ?? "-",
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                }).OrderBy(s => s.StartTime).ToList();
        }

        private async Task<bool> IsStudentBanned(string studentId)
        {
            if (await _studentBlockRepository.IsStudentBannedAsync(studentId))
            {
                StatusMessage = "Error: Your account is blocked. You cannot manage reservations.";
                return true;
            }
            return false;
        }

        public async Task<IActionResult> OnPostCancelAsync(int reservationId)
        {
            var student = await _userManager.GetUserAsync(User);
            if (student == null) return Forbid();
            if (await IsStudentBanned(student.Id)) return RedirectToPage();

            try
            {
                await _mediator.Send(new CancelReservationCommand(reservationId, student.Id));
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
            var student = await _userManager.GetUserAsync(User);
            if (student == null) return Forbid();
            if (await IsStudentBanned(student.Id)) return RedirectToPage();

            try
            {
                await _mediator.Send(new ChangeReservationCommand(currentReservationId, newReservationId, student.Id));
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