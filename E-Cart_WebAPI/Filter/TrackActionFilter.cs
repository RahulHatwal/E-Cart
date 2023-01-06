using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace E_Cart_WebAPI.Filter
{
    public class TrackActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            TrackAction("OnActionExecuted", filterContext.RouteData);
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            TrackAction("OnActionExecuting", filterContext.RouteData);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            TrackAction("OnResultExecuted", filterContext.RouteData);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            TrackAction("OnResultExecuting ", filterContext.RouteData);
        }
        private void TrackAction(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0}- controller:{1} action:{2}", methodName,
                                                                        controllerName,
                                                                        actionName);
            Debug.WriteLine(message);
        }
    }
}
