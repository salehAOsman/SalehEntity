using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFandDB.Models
{
    //I create here ViewModel to multiplay both classes "Student" as type class and course as "class list", this list nested inside student class but we can do that as separate classes  
    
    public class SudentCourseViewModel  //as we see here we have to use as key to know   -----ViewModel
    {
        
        public Student Student { get; set; }   // here we create just an object that has same name of student class
        public Course Course { get; set; }     // here we create just an object for Course class 
        //after we create both objects we will go to controller to create new object for this viewModel then we can send to view to display our result 
        //ther are another way to do as Ajax till exp to solve this idea 
        
    }
}