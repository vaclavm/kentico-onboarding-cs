﻿using System.Web.Http;

namespace ToDoList.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(RouteConfig.Register);
            GlobalConfiguration.Configure(DependencyInjectionConfig.Register);
            GlobalConfiguration.Configure(JsonConfig.Register);
        }
    }
}
