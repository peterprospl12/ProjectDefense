using MediatR;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Commands;
using System.Security;
using Microsoft.AspNetCore.Identity;
using ProjectDefense.Domain.Entities;
using ProjectDefense.Domain.Enums;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class CancelReservationCommandHandler(IReservationRepository reservationRepository, UserManager<User> _userManager)
        : IRequestHandler<CancelReservationCommand>
    {
        public async Task Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await reservationRepository.GetByIdAsync(request.ReservationId);

            if (reservation == null)
            {
                throw new KeyNotFoundException("The requested reservation slot does not exist.");
            }

            if (reservation.StudentId == null)
            {
                return;
            }

            if (reservation.StartTime <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot cancel a reservation that is in the past.");
            }

            var isAuthorized = false;
            var result = _userManager.FindByIdAsync(request.CancelerId).Result;
            if (result != null)
                switch (result.Role)
                {
                    case Role.Lecturer:
                        isAuthorized = true;
                        break;
                    case Role.Student:
                    {
                        if (reservation.StudentId == request.CancelerId)
                        {
                            isAuthorized = true;
                        }

                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            if (!isAuthorized)
            {
                throw new SecurityException("User is not authorized to cancel this reservation.");
            }

            reservation.StudentId = null;

            await reservationRepository.UpdateAsync(reservation);
        }
    }
}