using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;
using TimeTrackerCQRS.Infrastructure;
using System.Linq;

namespace TimeTrackerCQRS.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Task", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var container = Bootstrapper.InitializeContainer();
			container.Configure(x => x.For<IControllerFactory>().Use<ContainerControllerFactory>());
			DependencyResolver.SetResolver(new ContainerDependencyResolver(container));
        }
    }

	public class ContainerControllerFactory : DefaultControllerFactory
	{
		readonly IContainer container;

		public ContainerControllerFactory(IContainer container)
		{
			this.container = container;
		}

		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			if (controllerType == null) return null;

			return (IController)container.GetInstance(controllerType);
		}
	}

	public class ContainerDependencyResolver : IDependencyResolver
	{
		readonly IContainer container;

		public ContainerDependencyResolver(IContainer container)
		{
			this.container = container;
		}

		public object GetService(Type serviceType)
		{
			return container.TryGetInstance(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return container.GetAllInstances(serviceType).Cast<object>();
		}
	}
}