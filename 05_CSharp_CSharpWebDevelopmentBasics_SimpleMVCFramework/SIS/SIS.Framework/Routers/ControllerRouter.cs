using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api;
using SIS.WebServer.Results;
using RedirectResult = SIS.WebServer.Results.RedirectResult;

namespace SIS.Framework.Routers
{
    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var requestUrlSplit = request.Url.Split("/", StringSplitOptions.RemoveEmptyEntries);

            var requestMethod = request.RequestMethod.ToString();
            var controllerName = requestUrlSplit[0];
            var actionName = requestUrlSplit[1];

            var controller = GetController(controllerName, request);
            MethodInfo action = this.GetMethod(requestMethod, controller, actionName);

            if (action == null)
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.PrepareResponse(controller, action);
        }

        private IHttpResponse PrepareResponse(Controller controller, MethodInfo action)
        {
            IActionResult actionResult = (IActionResult) action.Invoke(controller, null);
            string invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocationResult, HttpResponseStatusCode.Ok);
            }
            else if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocationResult);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supported");
            }

        }

        private MethodInfo GetMethod(string requestMethod, Controller controller, string actionName)
        {
            MethodInfo method = null;

            foreach (var methodInfo in GetSuitableMethods(controller, actionName))
            {
                var attributes = methodInfo.GetCustomAttributes()
                    .Where(attr => attr is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();
                if (!attributes.Any() && requestMethod.ToUpper() == "GET")
                {
                    return methodInfo;
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.IsValid(requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return method;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller
                .GetType()
                .GetMethods()
                .Where(methodInfo => methodInfo.Name.ToLower() == actionName.ToLower());
        }

        public static Controller GetController(string controllerName, IHttpRequest request)
        {
            if (controllerName != null)
            {
                var fullyQualifiedControllerName = string.Format("{0}.{1}.{2}{3}, {0}",
                    MvcContext.Get.AssemblyName,
                    MvcContext.Get.ControllersFolder,
                    (controllerName[0].ToString().ToUpper() + controllerName.Substring(1)),
                    MvcContext.Get.ControllerSuffix);


                
                
                var controllerType = Type.GetType(fullyQualifiedControllerName, true);
                
                
                if (controllerType == null) return null;

                var controller = (Controller) Activator.CreateInstance(controllerType);

                if (controller != null)
                {
                    controller.Request = request;
                }

                return controller;  
            }

            return null;
        }
    }
}
