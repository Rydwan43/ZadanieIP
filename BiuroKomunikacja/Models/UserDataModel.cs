using System;
using BiuroKomunikacja.Areas.Identity;

namespace BiuroKomunikacja.Models
{
    public class UserDataModel
    {
        public Guid Id { get; set; }
        public string Dane { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime CzasTransakcji { get; set; }
        public decimal Kwota { get; set; }
    }
}
