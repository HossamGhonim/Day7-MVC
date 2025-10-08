// في مجلد MVC2.Middlewares.Filters/ExceptionHandleFilter.cs
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore.Mvc
{
    internal class ViewDataDictionary : ViewFeatures.ViewDataDictionary
    {
        public ViewDataDictionary(IModelMetadataProvider metadataProvider, ModelStateDictionary modelState) : base(metadataProvider, modelState)
        {
        }
    }
}