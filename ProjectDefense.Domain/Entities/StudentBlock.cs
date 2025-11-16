namespace ProjectDefense.Domain.Entities
{
    public class StudentBlock
    {
        public int Id { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTimeOffset? BlockUntil { get; set; }

        public string StudentId { get; set; } = null!;
        public User Student { get; set; } = null!;
    }
}