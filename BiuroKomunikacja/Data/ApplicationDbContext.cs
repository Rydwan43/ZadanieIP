using System;
using System.Collections.Generic;
using System.Text;
using BiuroKomunikacja.Areas.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BiuroKomunikacja.Models;

namespace BiuroKomunikacja.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BiuroKomunikacja.Models.DocsModel> DocsModel { get; set; }
        public DbSet<BiuroKomunikacja.Models.MessageModel> MessageModel { get; set; }
        public DbSet<BiuroKomunikacja.Models.UserDataModel> UserDataModel { get; set; }
    }
}
