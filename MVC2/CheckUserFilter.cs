using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVC2.Filters
{
    public class CheckUserFilter : Attribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // احصل على UserType من الـ Query String أو Header (كـ string)
            string? userType = context.HttpContext.Request.Query["UserType"].FirstOrDefault()
                            ?? context.HttpContext.Request.Headers["UserType"].FirstOrDefault();

            // تحقق من القيمة
            if (string.IsNullOrEmpty(userType) || userType != "Student")
            {
                context.Result = new ContentResult
                {
                    Content = "Access Denied! You must send header UserType=Student",
                    ContentType = "text/plain",
                    StatusCode = 403 // Forbidden
                };
                return;
            }
            else
            {
                // تمرير UserType للـ Action عن طريق HttpContext.Items
                context.HttpContext.Items["UserType"] = userType;
            }
        }
    }
}
