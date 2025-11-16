using Microsoft.EntityFrameworkCore;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Infrastructure.Persistence.Repositories
{
    public class StudentBlockRepository(ApplicationDbContext context) : IStudentBlockRepository
    {
        public async Task AddAsync(StudentBlock studentBlock)
        {
            await context.StudentBlocks.AddAsync(studentBlock);
            await context.SaveChangesAsync();
        }

        public async Task<bool> IsStudentBannedAsync(string studentId)
        {
            var now = DateTimeOffset.UtcNow;

            var isPermanentlyBanned = await context.StudentBlocks
                .AnyAsync(b => b.StudentId == studentId && b.BlockUntil == null);

            if (isPermanentlyBanned) return true;

            var blockUntilList = await context.StudentBlocks
                .Where(b => b.StudentId == studentId)
                .Select(b => b.BlockUntil)
                .ToListAsync();

            var hasActiveTemporaryBan = blockUntilList
                .Where(d => d.HasValue)
                .Any(d => d.Value > now);

            return hasActiveTemporaryBan;
        }

        public async Task<List<string>> GetAllBannedStudentIdsAsync()
        {
            var now = DateTimeOffset.UtcNow;

            var blocks = await context.StudentBlocks
                .Select(b => new { b.StudentId, b.BlockUntil })
                .ToListAsync();

            var permanentBanIds = blocks
                .Where(b => b.BlockUntil == null)
                .Select(b => b.StudentId);

            var temporaryBanIds = blocks
                .Where(b => b.BlockUntil.HasValue && b.BlockUntil.Value > now)
                .Select(b => b.StudentId);

            return permanentBanIds
                .Concat(temporaryBanIds)
                .Distinct()
                .ToList();
        }
    }
}
