using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Application.Interfaces
{
    public interface IAvailabilityRepository
    {
        Task AddAsync(LecturerAvailability availability);
        Task<bool> CheckForConflictsAsync(int roomId, DateTime start, DateTime end);
    }
}
