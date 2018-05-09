using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace seo_lucene.net_demo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //字典文件如果在Dict文件夹下，且默认配置，可以不用指定xml文件
            PanGu.Segment.Init(@"D:\学习代码\Seo-Lucene.Net\seo_lucene.net_demo\bin\PanGu\PanGu.xml");
        }
    }
}
