using EFandDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFandDB.Controllers
{
    public class HomeController : Controller
    {
        //We create controller to list It need name space  : ctr .
        SchoolDbContext db = new SchoolDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            //object for list to get data out of the tables
            List<Student> myStudent = db.Students.ToList();
            return View(myStudent);
        }

        //public ActionResult Create()
        //{
        //    Student myStudent = new Student();
                //    myStudent.Name = "Samer";
        //    myStudent.City = "Karlskrona";
        //    // we need to retern this info to data base by this code we have obj for dbase and name of table 
        //    db.Students.Add(myStudent);//add bobbo to DB

        //    db.SaveChanges();//saves the changes (add bobbo) may be we need to add many new informations then we do this code as "db.SaveChanges()"
        //    // then we redirect to action "index" 
        //    return RedirectToAction("Index");
        //}
    }
}