using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AddFooterFilter : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Controller is Controller controller)
        {
            // أضف الـ FooterMessage قبل الـ Result عشان يظهر في الـ View
            controller.ViewBag.FooterMessage = "Department added successfully.";
            controller.ViewBag.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        base.OnResultExecuting(context);
    }
    public override void OnResultExecuted(ResultExecutedContext context)
    {
    }
}
