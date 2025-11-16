namespace ProjectDefense.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsBlocked { get; set; }

        public int AvailabilityId { get; set; }
        public LecturerAvailability Availability { get; set; } = null!;

        public string? StudentId { get; set; }
        public User? Student { get; set; }
    }
}