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
    public class StudentsController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: Students
        public ActionResult Index(string orderBy)
        {
            List<Student> studentList = new List<Student>();
            if (string.IsNullOrEmpty(orderBy))
            {
                ViewBag.OrderNameBy = "NameA";
                studentList = db.Students.ToList();
            }
            else
            {
                switch (orderBy)
                {
                    case "NameA":
                        studentList = db.Students.OrderBy(s => s.Name).ToList();
                        ViewBag.OrderNameBy = "NameD";
                        break;

                    case "NameD":
                        studentList = db.Students.OrderByDescending(s => s.Name).ToList();
                        ViewBag.OrderNameBy = "NameA";
                        break;

                    default:

                        break;
                }
            }
            //we return this list to index to display 
            return View(studentList);
        }

        // GET: Students/Details/5  
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Include("Courses").SingleOrDefault(s => s.Id == id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        
        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,City")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,City")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }
        
        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]//DeleteCourseFromStudent
        public ActionResult DeleteCourseFromStudent(int? sId,int? cId)
        {
            if (sId == null || cId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            ViewBag.cId = cId; //Course Id
            //know after we create new Viewmodel we can create object for dealing both classes in view side   
            SudentCourseViewModel myViewModel = new SudentCourseViewModel();
            myViewModel.Student = db.Students.FirstOrDefault(s => s.Id == sId);//here we fitch our student 
            myViewModel.Course = db.Courses.FirstOrDefault(c => c.Id == cId);//here we fitch our course 
            
            if (myViewModel.Student == null || myViewModel.Course == null)
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
        public ActionResult ConfirmedDeleteCourseFromStudent(int? sId, int? cId)
        {
            if (sId == null || cId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.SingleOrDefault(c => c.Id == cId);
            if (course == null)
            {
                return HttpNotFound();
            }
            Student student = db.Students.Include("Courses").SingleOrDefault(s => s.Id == sId);
            if (student == null)
            {
                return HttpNotFound();
            }
            student.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = sId });
        }

        
        [HttpGet]
        public ActionResult AddCourseToStudent(int? sId)
        {
            if (sId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Course> courses = db.Courses.ToList();//reference to Couses list

            ViewBag.sId = sId;//Student Id
    
            return View(courses);
        }

        //5 create 'CourseToStudent' action 
        //6 change details view 
        [HttpGet]
        public ActionResult CourseToStudent(int? cId,int? sId)
        {
            Course course = db.Courses.SingleOrDefault(c => c.Id == cId);
            Student student = db.Students.Include("Courses").SingleOrDefault(s => s.Id == sId);
            student.Courses.Add(course);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = sId });
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
