namespace SE1611_Group_4_Final_Project.Models
{
    public partial class User
    {
        public Guid Id { get; set; }
        public Guid? RoomId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string IdentifyNumber { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual Room? Room { get; set; }
    }
}
