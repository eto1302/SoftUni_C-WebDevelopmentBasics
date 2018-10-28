using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using SIS.Framework.ActionResults;
using SIS.Framework.Models;
using SIS.Framework.Security;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Requests;

namespace SIS.Framework.Controllers
{
    public abstract class Controller
    {
        protected Controller()
        {
            this.Model = new ViewModel();
        }

        private ViewEngine ViewEngine { get;} = new ViewEngine();

        public IHttpRequest Request { get; set; }

        protected IViewable View([CallerMemberName] string actionName = "")
        {
            string controllerName = ControllerUtilities.GetControllerName(this);
            string viewContent = null;

            try
            {
                viewContent = this.ViewEngine.GetViewContent(controllerName, actionName);
            }
            catch (FileNotFoundException e)
            {
                this.Model.Data["Error"] = e.Message;

                viewContent = this.ViewEngine.GetErrorContent();
            }

            string renderedContent = this.ViewEngine.RenderHtml(viewContent, this.Model.Data);
            return new ViewResult(new View(renderedContent));
        }


        protected void SignIn(IIdentity auth)
        {
            this.Request.Session.AddParameter("auth", auth);
        }

        protected void SignOut()
        {
            this.Request.Session.ClearParameters();
        }

        public Model ModelState { get; } = new Model();

        protected ViewModel Model { get; }

        public IIdentity Identity => (IIdentity) this.Request.Session.GetParameter("auth");
        
        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);
    }
}
