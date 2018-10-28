using System;
using System.Collections.Generic;
using IRunesWebApp.Controllers;
using IRunesWebApp.Services;
using Services;
using SIS.Framework;
using SIS.Framework.Routers;
using SIS.Framework.Services;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;

namespace IRunesWebApp
{
    class Launcher
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());
        }
    }
}
