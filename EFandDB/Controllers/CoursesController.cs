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
    public class CoursesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();
        
        #region
        // GET: Courses
        public ActionResult Index(string orderBy)
        {
            List<Course> courseList = new List<Course>();
            if (string.IsNullOrEmpty(orderBy))
            {
                ViewBag.OrderNameBy = "NameA";
                courseList = db.Courses.ToList();
            }
            else
            {
                switch (orderBy)
                {
                    case "NameA":
                        courseList = db.Courses.OrderBy(s => s.Name).ToList();
                        ViewBag.OrderNameBy = "NameD";
                        break;

                    case "NameD":
                        courseList = db.Courses.OrderByDescending(s => s.Name).ToList();
                        ViewBag.OrderNameBy = "NameA";
                        break;

                    default:

                        break;
                }
            }

            return View(courseList);
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            /*
             * Course course = db.Courses.Find(id);
             * //WE have to change the code 
             * to this dawon if we have list inside this object because db does not call sublist then we would have null reference */
            Course course = db.Courses.Include("Assignments").SingleOrDefault(a => a.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
        
        //*
        //*1
        [HttpGet]
        public ActionResult AddAssignmentToCourse(int? cId)//Add Assignment To Course
        {
            if (cId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Assignment> assignments = db.Assignments.ToList();//reference to assignments list
            ViewBag.cId = cId;//assignment Id
            return View(assignments);
        }

        //*2
        [HttpGet]
        public ActionResult AssignmentToCourse(int? aId, int? cId)//Assignment To Course
        {
            Assignment assignment = db.Assignments.SingleOrDefault(a => a.Id == aId);
            Course course = db.Courses.Include("Assignments").SingleOrDefault(c => c.Id == cId);
            course.Assignments.Add(assignment);//assign assignment to list of course
            db.SaveChanges();

            return RedirectToAction("Details", new { id = cId });
        }
        //*3
        [HttpGet] //DeleteCourseFromStudent  DeleteAssignmentFromCourse
        public ActionResult DeleteAssignmentFromCourse(int? cId, int? aId)
        {
            if (cId == null || aId == null)
            { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            ViewBag.aId = aId; //assignment Id
            //know after we create new Viewmodel we can create object for dealing both classes in view side   
            AssignmentCourseViewModel myViewModel = new AssignmentCourseViewModel();
            myViewModel.Course = db.Courses.FirstOrDefault(c => c.Id == cId);//here we fitch our course 
            myViewModel.Assignment = db.Assignments.FirstOrDefault(a => a.Id == aId);//here we fitch our student 

            if (myViewModel.Assignment == null || myViewModel.Course == null)
            {
                return HttpNotFound();
            }
            //now we return viewmodel object to draw both 
            return View(myViewModel);  // View 
        }

        //*4
        [HttpGet]//ConfirmedDeleteAssignmentFromCourse
        public ActionResult ConfirmedDeleteAssignmentFromCourse(int? aId, int? cId)
        {
            if (aId == null || cId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.SingleOrDefault(a => a.Id == aId);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            Course course = db.Courses.Include("Assignment").SingleOrDefault(c => c.Id == cId);
            if (course == null)
            {
                return HttpNotFound();
            }
            course.Assignments.Remove(assignment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = aId });
        }
        //*
        
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
