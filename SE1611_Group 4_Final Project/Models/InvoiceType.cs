﻿namespace SE1611_Group_4_Final_Project.Models
{
    public partial class InvoiceType : Entity
    {
        public InvoiceType()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
