using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Application.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync(int reservationId);
        Task<IEnumerable<Reservation>> GetByIdsAsync(params int[] reservationIds); 
        Task<bool> HasStudentAlreadyBookedAsync(string studentId);
        Task<IEnumerable<Reservation>> GetReservationsForLecturerInPeriodAsync(string lecturerId, DateTime start, DateTime end);
        Task<IEnumerable<Reservation>> GetActiveReservationsByStudentIdAsync(string studentId);
        Task<IEnumerable<Reservation>> GetAllByLecturerIdAsync(string lecturerId);
        Task<IEnumerable<Reservation>> GetAvailableSlotsAsync(DateTime? fromDate);
        Task UpdateAsync(Reservation reservation);
        Task UpdateRangeAsync(IEnumerable<Reservation> reservations);
    }
}