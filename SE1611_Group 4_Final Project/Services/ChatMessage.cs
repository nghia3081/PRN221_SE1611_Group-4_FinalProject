using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Services
{
    public class ChatMessage
    {
        public Guid Id { get; set; } 
        public User user { get; set; }
        public string Text { get; set; }
    }
}
