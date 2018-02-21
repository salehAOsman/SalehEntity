using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFandDB.Models
{
    public class Teacher
    {
       
        public int Id { get; set; }

        [Required(ErrorMessage = "the name of teacher")]
        public string Name { get; set; }

        [Required(ErrorMessage = "the name of city")]
        public string City { get; set; }

        //relation * ---> *  there is list in Course class
        public List<Course> Courses { get; set; }
    }
}