using System;
using System.Collections.Generic;

namespace SE1611_Group_4_Final_Project.Models
{
    public partial class Invoice
    {
        public Guid Id { get; set; }
        public int? TypeId { get; set; }
        public string Title { get; set; } = null!;
        public decimal GrandTotal { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? Description { get; set; }
        public int? PaymentStatusId { get; set; }
        public Guid? RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? UserName { get; set; }

        public virtual PaymentStatus? PaymentStatus { get; set; }
        public virtual Room? Room { get; set; }
        public virtual InvoiceType? Type { get; set; }
    }
}
