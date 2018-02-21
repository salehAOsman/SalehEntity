using System;
using System.Collections.Generic;
using System.Data.Entity;//relates with DbContext class
using System.Linq;
using System.Web;

namespace EFandDB.Models
{
    //reference to DataBase
    public class SchoolDbContext:DbContext
    {
        public SchoolDbContext() :base("name=SchoolDbContext")
        {
        }
        
        //dbSet object to representing rows inside Dbase tables
        //public DbSet<Course> Courses { get; set; }
        //public DbSet<Assignment> Assignments { get; set; }
        //public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Teacher> Teachers { get; set; }
    }
}