using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFandDB.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "the name of student")]
        public string Name { get; set; }
        [Required(ErrorMessage = "the name of city")]
        public string City { get; set; }

        // relation    * -----> *
        public List<Course> Courses {get;set;}
    }
}