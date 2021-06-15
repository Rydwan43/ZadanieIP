using System;
using System.ComponentModel.DataAnnotations;
using BiuroKomunikacja.Areas.Identity;

namespace BiuroKomunikacja.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

        virtual public ApplicationUser sender { get; set; }
        virtual public ApplicationUser reciver { get; set; }

        public string Message { get; set; }
    
        [DataType(DataType.Date)]
        public DateTime MessageDate { get; set; }
    }
}
