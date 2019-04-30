using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class Assignment2DataContext : DbContext
    {
        public Assignment2DataContext(DbContextOptions<Assignment2DataContext> options)
            : base(options)
        {

        }

        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<BadWord> BadWord { get; set; }
        public DbSet<Photo> Photo { get; set; }


    }



}
