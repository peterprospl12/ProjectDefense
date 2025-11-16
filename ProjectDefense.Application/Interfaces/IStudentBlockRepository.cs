using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Application.Interfaces
{
    public interface IStudentBlockRepository
    {
        Task AddAsync(StudentBlock studentBlock);
        Task<bool> IsStudentBannedAsync(string studentId);
        Task<List<string>> GetAllBannedStudentIdsAsync();
    }
}