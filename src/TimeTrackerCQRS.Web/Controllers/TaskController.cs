using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTrackerCQRS.Commands;
using TimeTrackerCQRS.Infrastructure;
using TimeTrackerCQRS.ViewModel;

namespace TimeTrackerCQRS.Web.Controllers
{
    public class TaskController : Controller
    {
        //
        // GET: /Task/

        public ActionResult Index(int skip = 0, int take = 10)
        {
            var query = from t in ServiceLocator.PersistentViewModel.Query<TaskListItem>()
                        orderby t.LastStartTime descending
                        select t;
            return View(query.Skip(skip).Take(take));
        }

        //
        // GET: /Task/Details/5

        public ActionResult Details(Guid id)
        {
            return View();
        }

        //
        // GET: /Task/Create

        public ActionResult Create()
        {
            return View(new CreateTask{Id = Guid.NewGuid()});
        } 

        //
        // POST: /Task/Create

        [HttpPost]
        public ActionResult Create(CreateTask createTask)
        {
            ServiceLocator.CommandSender.Send(createTask);
            //return RedirectToAction("Details", new {id = createTask.Id});
            return RedirectToAction("Index");
        }
        
        //
        // GET: /Task/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            return View();
        }

        //
        // POST: /Task/Edit/5

        [HttpPost]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Task/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Task/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
