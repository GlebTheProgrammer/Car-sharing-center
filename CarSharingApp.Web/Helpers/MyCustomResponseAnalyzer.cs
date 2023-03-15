using Azure.Core;
using CarSharingApp.Web.Helpers.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace CarSharingApp.Web.Helpers
{
    public static class MyCustomResponseAnalyzer
    {
        public static IActionResult? Analize<T>(MyControllerContext myControllerContext, 
                                                HttpResponseMessage? response,
                                                string onErrorStatusCode_ViewName,
                                                T? onErrorStatusCode_ViewModel,
                                                bool isReturningPartialView = false)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    {
                        myControllerContext.Controller.HttpContext.Session.SetString("Unauthorized401Error", "true");
                        break;
                    }

                case HttpStatusCode.Forbidden:
                    {
                        myControllerContext.Controller.HttpContext.Session.SetString("Forbidden403Error", "true");
                        break;
                    }

                case HttpStatusCode.NotFound:
                    {
                        myControllerContext.Controller.HttpContext.Session.SetString("NotFound404Error", "true");
                        break;
                    }

                case HttpStatusCode.InternalServerError:
                    {
                        myControllerContext.Controller.HttpContext.Session.SetString("InternalServer500Error", "true");
                        return myControllerContext.RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = 500 });
                    }

                default:
                    {
                        response.EnsureSuccessStatusCode();
                        return null;
                    }
            }

            if (isReturningPartialView)
                return onErrorStatusCode_ViewModel is null ? myControllerContext.PartialView(onErrorStatusCode_ViewName)
                                                           : myControllerContext.PartialView(onErrorStatusCode_ViewName, onErrorStatusCode_ViewModel);
            else
                return onErrorStatusCode_ViewModel is null ? myControllerContext.View(onErrorStatusCode_ViewName)
                                                           : myControllerContext.View(onErrorStatusCode_ViewName, onErrorStatusCode_ViewModel);
        }
    }
}
