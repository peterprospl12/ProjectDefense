using MediatR;
using Microsoft.AspNetCore.Identity;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Domain.Entities;
using ProjectDefense.Domain.Enums;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class BanStudentCommandHandler(
        UserManager<User> userManager,
        IReservationRepository reservationRepository,
        IStudentBlockRepository studentBlockRepository)
        : IRequestHandler<BanStudentCommand>
    {
        public async Task Handle(BanStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await userManager.FindByIdAsync(request.StudentId);
            if (student == null)
            {
                throw new KeyNotFoundException("The specified student does not exist.");
            }

            var roles = await userManager.GetRolesAsync(student);
            if (!roles.Contains(nameof(Role.Student)))
            {
                throw new InvalidOperationException("The specified user is not a student and cannot be banned.");
            }

            var studentBlock = new StudentBlock
            {
                StudentId = request.StudentId,
                Reason = request.Reason,
                BlockUntil = request.BanUntil 
            };
            await studentBlockRepository.AddAsync(studentBlock);

            var activeReservations = await reservationRepository.GetActiveReservationsByStudentIdAsync(request.StudentId);
            foreach (var reservation in activeReservations)
            {
                reservation.StudentId = null;
            }
            await reservationRepository.UpdateRangeAsync(activeReservations);

            var lockoutEnd = request.BanUntil ?? DateTimeOffset.MaxValue;
            var result = await userManager.SetLockoutEndDateAsync(student, lockoutEnd);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to set lockout for student: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}