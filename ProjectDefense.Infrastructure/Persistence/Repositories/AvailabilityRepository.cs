using Microsoft.EntityFrameworkCore;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Infrastructure.Persistence.Repositories
{
    public class AvailabilityRepository(ApplicationDbContext context) : IAvailabilityRepository
    {
        public async Task AddAsync(LecturerAvailability availability)
        {
            await context.LecturerAvailabilities.AddAsync(availability);
            await context.SaveChangesAsync();
        }

        public async Task<bool> CheckForConflictsAsync(int roomId, DateTime start, DateTime end)
        {
            return await context.LecturerAvailabilities
                .AnyAsync(a => a.RoomId == roomId && start < a.EndDate && end > a.StartDate);
        }
    }
}