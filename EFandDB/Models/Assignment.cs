using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace EFandDB.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "the name of assinment")]
        public string Name { get; set; }

        [Required(ErrorMessage = "the description of assinment")]
        public string Descriptions { get; set; }
        //relation   * ---> 1
        public Course AssigmedTo { get; set; } //Whay I use this name "AssigmedTo" I did not use it in all code
    }
}