using OSUDental.Messaging;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OSUDental
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        CacheMockService cms = new CacheMockService();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            cms.RegisterCacheEntry();
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			// If the dummy page is hit, then it means we want to add another item
			// in cache
			if( HttpContext.Current.Request.Url.ToString() == CacheMockService.DummyPageUrl )
			{
				// Add the item in cache and when succesful, do the work.
                cms.RegisterCacheEntry();
			}
		}
    }
}