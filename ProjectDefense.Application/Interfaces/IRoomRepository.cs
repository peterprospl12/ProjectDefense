using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Application.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(int id);
        Task<IEnumerable<Room>> GetAllAsync();
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(int id);
        Task<bool> IsRoomInUseAsync(int id);
    }
}