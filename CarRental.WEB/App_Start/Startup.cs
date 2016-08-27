using CarRental.Auth.BLL.Interfaces;
using CarRental.Auth.BLL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using NLog;
using Owin;

namespace CarRental.WEB
{
    /// <summary>
    /// Starts with the application
    /// </summary>
    public class Startup
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly IServiceCreator _serviceCreator = new ServiceCreator();

        public void Configuration(IAppBuilder app)
        {
            Logger.Debug("Startup configuration starts");
            app.CreatePerOwinContext(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
            Logger.Debug("Startup configuration finished");
        }

        private IUserService CreateUserService()
        {
            return _serviceCreator.CreateUserService("AuthContext");
        }
    }
}