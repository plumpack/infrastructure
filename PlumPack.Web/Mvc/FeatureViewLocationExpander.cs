using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace PlumPack.Web.Mvc
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var controllerActionDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return viewLocations;
            if(!controllerActionDescriptor.Properties.ContainsKey("feature")) return viewLocations;
            var featureName = controllerActionDescriptor.Properties["feature"] as string;
            if (string.IsNullOrEmpty(featureName)) return viewLocations;
            string controllerName = controllerActionDescriptor.ControllerName;

            return new List<string>
            {
                $"/Areas/{{2}}/Features/{featureName}/Views/{{0}}.cshtml",
                $"/Areas/{{2}}/Features/{featureName}/{controllerName}/Views/{{0}}.cshtml",
                "/Areas/{2}/Features/Shared/{0}.cshtml",
                $"/Features/{featureName}/Views/{{0}}.cshtml",
                $"/Features/{featureName}/{controllerName}/Views/{{0}}.cshtml",
                "/Features/Shared/{0}.cshtml"
            }.Union(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}