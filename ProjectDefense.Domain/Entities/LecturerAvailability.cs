namespace ProjectDefense.Domain.Entities
{
    public class LecturerAvailability
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    
        public string? LecturerId { get; set; }
        public User? Lecturer { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int SlotDurationInMinutes { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
