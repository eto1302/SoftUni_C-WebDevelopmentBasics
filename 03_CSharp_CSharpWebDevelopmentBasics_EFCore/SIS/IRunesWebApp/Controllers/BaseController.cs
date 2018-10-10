using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using IRunesWebApp.Data;
using Services;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;

namespace IRunesWebApp.Controllers
{
    public abstract class BaseController
    {
        protected IDictionary<string, string> ViewBag { get; set; }

        private const string RootDirectoryRelativePath = "../../../";

        private const string ViewsFolderName = "Views";

        private const string HtmlFileExtension = ".html";

        private const string DirectorySeparator = "/";

        private string GetCurrentControllerName() =>
            this.GetType().Name.Replace("Controller", string.Empty);

        protected IRunesContext Context { get; set; }

        protected readonly UserCookieService userCookieService;

        public BaseController()
        {
            this.Context = new IRunesContext();
            this.userCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        protected bool IsAuthenticated(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }

        public void SignInUser(string username, IHttpResponse response, IHttpRequest request)
        {
            request.Session.AddParameter("username", username);
            var UserCookieValue = this.userCookieService.GetUserCookie(username);
            response.Cookies.Add(new HttpCookie("IRunes_auth", UserCookieValue));
        }

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            

            string filePath = RootDirectoryRelativePath +
                ViewsFolderName +
                DirectorySeparator +
                this.GetCurrentControllerName() +
                DirectorySeparator +
                viewName +
                HtmlFileExtension;

            

            if (!File.Exists(filePath))
            {
                return new BadRequestResult(
                    $"View {viewName} not found.",
                    HttpResponseStatusCode.NotFound);
            }

            var fileContent = File.ReadAllText(filePath);

            foreach (var viewBagKey in ViewBag.Keys) 
            {
                var dynamicDataPlaceHolder = $"{{{{{viewBagKey}}}}}";
                if (fileContent.Contains(dynamicDataPlaceHolder))
                {
                    fileContent = fileContent.Replace(
                        dynamicDataPlaceHolder, 
                        this.ViewBag[viewBagKey]);

                }
            }

            var response = new HtmlResult(fileContent, HttpResponseStatusCode.Ok);

            return response;
        }
    }
}
