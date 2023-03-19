using System;
using System.Collections.Generic;

namespace SE1611_Group_4_Final_Project.Models
{
    public partial class Notification
    {
        public Notification()
        {
            Rooms = new HashSet<Room>();
        }

        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? HideDate { get; set; }
        public bool IsUse { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
