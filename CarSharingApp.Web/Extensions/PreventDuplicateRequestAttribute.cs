using Microsoft.AspNetCore.Mvc.Filters;

namespace CarSharingApp.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class PreventDuplicateRequestAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Form.ContainsKey("__RequestVerificationToken"))
            {
                await context.HttpContext.Session.LoadAsync();

                var currentToken = context.HttpContext.Request.Form["__RequestVerificationToken"].ToString();
                var lastToken = context.HttpContext.Session.GetString("LastProcessedToken");

                if (currentToken != null && currentToken.Equals(lastToken))
                {
                    context.ModelState.AddModelError(string.Empty, "Looks like you accidentally submitted the same form twice.");
                }
                else
                {
                    context.HttpContext.Session.SetString("LastProcessedToken", currentToken ?? throw new ArgumentNullException(nameof(currentToken)));
                    await context.HttpContext.Session.CommitAsync();
                }
            }

            await next();
        }
    }
}
