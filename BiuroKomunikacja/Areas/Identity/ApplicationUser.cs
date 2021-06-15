using System;
using Microsoft.AspNetCore.Identity;

namespace BiuroKomunikacja.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        public bool IsAdmin { get; set; } = false;
        public bool IsEmployee { get; set; } = false;

        public string RelatedUserId { get; set; }
        public virtual ApplicationUser RelatedUser { get; set; }
    }
}
