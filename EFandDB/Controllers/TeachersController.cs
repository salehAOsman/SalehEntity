using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EFandDB.Models;

namespace EFandDB.Controllers
{
    public class TeachersController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();
        
        // GET: Teachers
        public ActionResult Index(string orderBy)
        {
            List<Teacher> teacherList = new List<Teacher>();//we need teacher list to display

            if (string.IsNullOrEmpty(orderBy))
            {
                ViewBag.OrderNameBy = "NameA";
                teacherList = db.Teachers.ToList();//Fitch list of teachers 
            }
            else
            {
                switch (orderBy)
                {
                    case "NameA":
                        teacherList = db.Teachers.OrderBy(t => t.Name).ToList();//A --> Z
                        ViewBag.OrderNameBy = "NameD";
                        break;
                    case "NameD":
                        teacherList = db.Teachers.OrderByDescending(t => t.Name).ToList();//Z --> A
                        ViewBag.OrderNameBy = "NameA";
                        break;

                    default:

                        break;
                }
            }//To teacher view to display as ordering up or dawon 
            return View(teacherList);
        }

        [HttpGet]
        public ActionResult CourseToTeacher(int? tId,int? cId)
        {
            Course course = db.Courses.SingleOrDefault(c => c.Id == cId);
            Teacher teacher = db.Teachers.Include("Courses").SingleOrDefault(t => t.Id == tId);//we will find the teacher including his courses 
            teacher.Courses.Add(course); //we added here this course to list of teacher
            db.SaveChanges();

            return RedirectToAction("Details",new { id=tId});// can we add here a value like this  "id=teacher.Id"
        }

        [HttpGet]
        public ActionResult AddCourseToTeacher(int? tId)
        {
            if (tId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Course> courses = db.Courses.ToList(); //we need to fitch the list of courses from db.courses and converted as list  
            ViewBag.tId = tId; //Student Id
            return View(courses);
        }
        
        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Teacher teacher = db.Teachers.Find(id);//we have to change the code to be include courses
            Teacher teacher = db.Teachers.Include("Courses").SingleOrDefault(t=>t.Id==id);//To not have null as courses
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,City")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,City")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpGet]//DeleteCourseFromTeacher
        public ActionResult DeleteCourseFromTeacher(int? tId, int? cId)
        {
            if (tId == null || cId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.cId = cId; //Course Id
            //know after we create new Viewmodel we can create object for dealing both classes in view side   
            TeacherCourseViewModel myViewModel = new TeacherCourseViewModel();
            myViewModel.Teacher = db.Teachers.FirstOrDefault(s => s.Id == tId);//here we fitch our student 
            myViewModel.Course = db.Courses.FirstOrDefault(c => c.Id == cId);//here we fitch our course 

            if (myViewModel.Teacher == null || myViewModel.Course == null)
            {
                return HttpNotFound();
            }
            //now we return viewmodel object to draw both 
            return View(myViewModel);  // View 
        }
        //3 we will create 'AddCourseToStudent'
        //GET: Students/AddCourseToStudent
        //4 create view 

        [HttpGet]//ConfirmedDeleteCourseFromStudent
        public ActionResult ConfirmedDeleteCourseFromTeacher(int? tId, int? cId)
        {
            if (tId == null || cId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.SingleOrDefault(c => c.Id == cId); // fitch course from teacher list
            if (course == null)
            {
                return HttpNotFound();
            }
            Teacher teacher = db.Teachers.Include("Courses").SingleOrDefault(t => t.Id == tId);//to fitch object include list of course
            if (teacher == null)
            {
                return HttpNotFound();
            }
            teacher.Courses.Remove(course);//delete from list of teacher
            db.SaveChanges();
            return RedirectToAction("Details", new { id = tId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
