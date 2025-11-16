namespace ProjectDefense.Application.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBooked => !string.IsNullOrEmpty(StudentId);
        public string? StudentId { get; set; }
        public bool IsBlocked { get; set; }
    }
}