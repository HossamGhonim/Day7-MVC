using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MVC2.Models;

namespace MVC2
{
    public class CheckLocationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.ContainsKey("department"))
            {
                var department = context.ActionArguments["department"] as Department;

                if (department != null && department.Location != "EG" && department.Location != "USA")
                {
                    context.Result = new ContentResult
                    {
                        Content = "Invalid Location! Must be EG or USA.",
                        ContentType = "text/plain",
                        StatusCode = 400
                    };
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
