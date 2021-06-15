using System;
using BiuroKomunikacja.Areas.Identity;

namespace BiuroKomunikacja.Models
{
    public class DocsModel
    {
        public Guid Id { get; set; }
        public string DocumentUrl { get; set; }
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string EmployeeId { get; set; }

        public virtual ApplicationUser Client { get; set; }
        public virtual ApplicationUser Employee { get; set; }
    }
}
