using MediatR;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class BookReservationCommandHandler(IReservationRepository reservationRepository, IStudentBlockRepository studentBlockRepository)
        : IRequestHandler<BookReservationCommand>
    {
        public async Task Handle(BookReservationCommand request, CancellationToken cancellationToken)
        {
            var studentIsBanned = await studentBlockRepository.IsStudentBannedAsync(request.StudentId);
            if (studentIsBanned)
            {
                throw new InvalidOperationException("Student is banned");
            }
            var studentHasBooking = await reservationRepository.HasStudentAlreadyBookedAsync(request.StudentId);
            if (studentHasBooking)
            {
                throw new InvalidOperationException("Student has already booked a slot.");
            }

            var reservation = await reservationRepository.GetByIdAsync(request.ReservationId);

            if (reservation == null)
            {
                throw new KeyNotFoundException("The requested reservation slot does not exist.");
            }

            if (reservation.StudentId != null)
            {
                throw new InvalidOperationException("This slot has already been booked.");
            }

            if (reservation.StartTime <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot book a slot that is in the past.");
            }

            reservation.StudentId = request.StudentId;

            await reservationRepository.UpdateAsync(reservation);
        }
    }
}