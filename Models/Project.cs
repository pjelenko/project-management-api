namespace ProjectManagement.Models
{
    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public required User Owner { get; set; }
        public ICollection<Task> Tasks { get; set; } = [];
    }
}
