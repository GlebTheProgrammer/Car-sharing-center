using CarSharingApp.Web.Helpers.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Web.Primitives
{
    public abstract class WebAppController : Controller
    {
        protected MyControllerContext GenerateControllerContext()
        {
            return new MyControllerContext()
            {
                Request = Request,
                Response = Response,
                Controller = this,

                ViewModelResult = (viewName, model) =>
                {
                    return View(viewName, model);
                },

                ViewResult = (viewName) =>
                {
                    return View(viewName);
                },

                RedirectToActionModelResult = (actionName, controllerName, model) =>
                {
                    return RedirectToAction(actionName, controllerName, model);
                },

                RedirectToActionResult = (actionName, controllerName) =>
                {
                    return RedirectToAction(actionName, controllerName);
                },

                RedirectResult = (url) =>
                {
                    return Redirect(url);
                },

                PartialViewResult = (viewName) =>
                {
                    return PartialView(viewName);
                },

                PartialViewModelResult = (viewName, model) =>
                {
                    return PartialView(viewName, model);
                }
            };
        }
    }
}
