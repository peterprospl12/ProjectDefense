using Microsoft.EntityFrameworkCore;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Infrastructure.Persistence.Repositories
{
    public class RoomRepository(ApplicationDbContext context) : IRoomRepository
    {
        public async Task<Room> GetByIdAsync(int id)
        {
            return await context.Rooms.FindAsync(id);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await context.Rooms.OrderBy(r => r.Name).ToListAsync();
        }

        public async Task AddAsync(Room room)
        {
            await context.Rooms.AddAsync(room);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            context.Rooms.Update(room);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var room = await context.Rooms.FindAsync(id);
            if (room != null)
            {
                context.Rooms.Remove(room);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsRoomInUseAsync(int id)
        {
            return await context.LecturerAvailabilities
                .AnyAsync(a => a.RoomId == id && a.EndDate > DateTime.UtcNow);
        }
    }
}