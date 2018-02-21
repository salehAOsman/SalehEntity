using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFandDB.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "the name of course")]
        public string Name { get; set; }

        [Required(ErrorMessage = "the description of course")]
        public string Description { get; set; }

        //relation  1----> *  
        public List<Assignment> Assignments { get; set; }

        //relation  *<--->* there is list in teacher class
        public List<Teacher> Teachers { get; set; }

        //relation  *<--->*  there is list in student class
        public List<Student> Students { get; set; }
    }
}