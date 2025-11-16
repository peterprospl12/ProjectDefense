namespace ProjectDefense.Application.DTOs
{
    public class RoomDto
    {
        public RoomDto()
        {
        }

        public RoomDto(int id, string name, string number)
        {
            Id = id;
            Name = name;
            Number = number;
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
    }
}