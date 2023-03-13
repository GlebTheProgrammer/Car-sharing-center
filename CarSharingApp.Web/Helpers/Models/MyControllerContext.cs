using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Web.Helpers.Models
{
    public struct MyControllerContext
    {
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public Controller Controller { get; set; }

        public Func<string, object, ViewResult> ViewModelResult;
        public ViewResult View(string viewName, object model)
        {
            return this.ViewModelResult(viewName, model);
        }

        public Func<string, ViewResult> ViewResult;
        public ViewResult View(string viewName)
        {
            return this.ViewResult(viewName);
        }

        public Func<string, string, object, RedirectToActionResult> RedirectToActionModelResult;
        public RedirectToActionResult RedirectToAction(string actionName, string controllerName, object model)
        {
            return this.RedirectToActionModelResult(actionName, controllerName, model);
        }

        public Func<string, string, RedirectToActionResult> RedirectToActionResult;
        public RedirectToActionResult RedirectToAction(string actionName, string controllerName)
        {
            return this.RedirectToActionResult(actionName, controllerName);
        }

        public Func<string, RedirectResult> RedirectResult;
        public RedirectResult Redirect(string url)
        {
            return this.RedirectResult(url);
        }

        public Func<string, object, PartialViewResult> PartialViewModelResult;
        public PartialViewResult PartialView(string viewName, object model)
        {
            return this.PartialViewModelResult(viewName, model);
        }

        public Func<string, PartialViewResult> PartialViewResult;
        public PartialViewResult PartialView(string viewName)
        {
            return this.PartialViewResult(viewName);
        }
    }
}
