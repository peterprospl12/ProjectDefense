using Microsoft.EntityFrameworkCore;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Infrastructure.Persistence.Repositories
{
    public class ReservationRepository(ApplicationDbContext context) : IReservationRepository
    {
        public async Task<Reservation?> GetByIdAsync(int reservationId)
        {
            return await context.Reservations
                .Include(r => r.Availability)
                .ThenInclude(a => a.Lecturer)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(r => r.Id == reservationId);
        }

        public async Task<IEnumerable<Reservation>> GetByIdsAsync(params int[] reservationIds)
        {
            return await context.Reservations
                .Where(r => reservationIds.Contains(r.Id))
                .Include(r => r.Availability)
                .ThenInclude(a => a.Lecturer)
                .ToListAsync();
        }

        public async Task<bool> HasStudentAlreadyBookedAsync(string studentId)
        {
            return await context.Reservations
                .AnyAsync(r => r.StudentId == studentId && r.StartTime > DateTime.UtcNow);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsForLecturerInPeriodAsync(string lecturerId, DateTime start, DateTime end)
        {
            return await context.Reservations
                .Include(r => r.Student) // Dołącz dane studenta
                .Include(r => r.Availability) // Dołącz dane o dostępności
                .ThenInclude(a => a.Room) // W ramach dostępności, dołącz dane o pokoju
                .Where(r => r.Availability.LecturerId == lecturerId && r.StartTime >= start && r.EndTime <= end)
                .OrderBy(r => r.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetActiveReservationsByStudentIdAsync(string studentId)
        {
            return await context.Reservations
                .Where(r => r.StudentId == studentId && r.StartTime > DateTime.UtcNow)
                .Include(r => r.Availability)
                .ThenInclude(a => a.Lecturer)
                .Include(r => r.Availability.Room)
                .OrderBy(r => r.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllByLecturerIdAsync(string lecturerId)
        {
            return await context.Reservations
                .Where(r => r.Availability.LecturerId == lecturerId)
                .Include(r => r.Availability)
                .ThenInclude(a => a.Lecturer)
                .Include(r => r.Availability)
                .ThenInclude(a => a.Room)
                .Include(r => r.Student)
                .OrderByDescending(r => r.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAvailableSlotsAsync(DateTime? fromDate)
        {
            var query = context.Reservations
                .Where(r => r.StudentId == null && r.StartTime > DateTime.UtcNow);

            if (fromDate.HasValue)
            {
                query = query.Where(r => r.StartTime.Date >= fromDate.Value.Date);
            }

            return await query
                .Include(r => r.Availability)
                .ThenInclude(a => a.Lecturer)
                .Include(r => r.Availability.Room)
                .OrderBy(r => r.StartTime)
                .ToListAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            context.Reservations.Update(reservation);
            await context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Reservation> reservations)
        {
            context.Reservations.UpdateRange(reservations);
            await context.SaveChangesAsync();
        }
    }
}
