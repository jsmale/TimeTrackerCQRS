using System;
using System.Linq;
using System.Web.Mvc;
using TimeTrackerCQRS.Commands;
using TimeTrackerCQRS.Infrastructure;
using TimeTrackerCQRS.ViewModel;

namespace TimeTrackerCQRS.Web.Controllers
{
    public class TaskController : Controller
    {
        public ActionResult Index(int skip = 0, int take = 10)
        {
            using (var viewModel = ServiceLocator.PersistentViewModelFactory.GetPersitentViewModel())
            {
                var query = from t in viewModel.Query<TaskListItem>()
                            orderby t.LastStartTime descending
                            select t;
                return View(query.Skip(skip).Take(take));
            }
        }

        public ActionResult Create()
        {
            return View(new CreateTask{Id = Guid.NewGuid()});
        } 

        [HttpPost]
        public ActionResult Create(CreateTask createTask)
        {
            ServiceLocator.CommandSender.Send(createTask);
            return RedirectToAction("Details", new {id = createTask.Id});
        }

        public ActionResult Details(Guid id)
        {
            using (var viewModel = ServiceLocator.PersistentViewModelFactory.GetPersitentViewModel())
            {
                var taskDetail = (from t in viewModel.Query<TaskDetail>()
                                  where t.Id == id
                                  select t).Single();
                return View(taskDetail);
            }
        }

        [HttpPost]
        public ActionResult Start(StartTask startTask)
        {
            ServiceLocator.CommandSender.Send(startTask);
            return RedirectToAction("Details", new {id = startTask.CommandId});
        }

        [HttpPost]
        public ActionResult Stop(StopTask stopTask)
        {
            ServiceLocator.CommandSender.Send(stopTask);
            return RedirectToAction("Details", new { id = stopTask.CommandId });
        }  
    }
}
