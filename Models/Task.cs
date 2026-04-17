namespace ProjectManagement.Models
{
    public class Task
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Points { get; set; }
        public int ProjectId { get; set; }
        public required Project Project { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; }
        public Enums.TaskStatus Status { get; set; }
    }
}