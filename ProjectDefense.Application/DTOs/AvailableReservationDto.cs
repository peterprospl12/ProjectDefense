namespace ProjectDefense.Application.DTOs;

public class AvailableReservationDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string RoomName { get; set; }
    public string RoomNumber { get; set; }
}