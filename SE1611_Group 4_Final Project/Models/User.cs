using System;
using System.Collections.Generic;

namespace SE1611_Group_4_Final_Project.Models
{
    public partial class User
    {
        public User()
        {
            Invoices = new HashSet<Invoice>();
        }

        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string IdentifyNumber { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
