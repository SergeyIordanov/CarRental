using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace CarRental.Tests.WEB.Fakes
{
    public class FakeControllerContext : ControllerContext
    {
        /// <summary>
        /// Create an empty context
        /// </summary>
        /// <param name="controller">Contoller you want to create context for</param>
        public FakeControllerContext(ControllerBase controller)
            : this(controller, null, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Context with preset cookies
        /// </summary>
        /// <param name="controller">Contoller you want to create context for</param>
        /// <param name="cookies">Cookies values</param>
        public FakeControllerContext(ControllerBase controller, HttpCookieCollection cookies)
            : this(controller, null, null, null, null, cookies, null)
        {
        }

        public FakeControllerContext(ControllerBase controller, SessionStateItemCollection sessionItems)
            : this(controller, null, null, null, null, null, sessionItems)
        {
        }

        /// <summary>
        /// Context with preset form params
        /// </summary>
        /// <param name="controller">Contoller you want to create context for</param>
        /// <param name="formParams">Form params for context</param>
        public FakeControllerContext(ControllerBase controller, NameValueCollection formParams)
            : this(controller, null, null, formParams, null, null, null)
        {
        }

        /// <summary>
        /// Context with preset form & query string params
        /// </summary>
        /// <param name="controller">Contoller you want to create context for</param>
        /// <param name="formParams">Form params for context</param>
        /// <param name="queryStringParams">Params from query string for context</param>
        public FakeControllerContext(ControllerBase controller, NameValueCollection formParams, NameValueCollection queryStringParams)
            : this(controller, null, null, formParams, queryStringParams, null, null)
        {
        }

        /// <summary>
        /// Context with preset identity user name
        /// </summary>
        /// <param name="controller">Contoller you want to create context for</param>
        /// <param name="userName">Identity user name</param>
        public FakeControllerContext(ControllerBase controller, string userName)
            : this(controller, userName, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Context with preset identity user name & roles
        /// </summary>
        /// <param name="controller">Contoller you want to create context for</param>
        /// <param name="userName">Identity user name</param>
        /// <param name="roles">Roles for context</param>
        public FakeControllerContext(ControllerBase controller, string userName, string[] roles)
            : this(controller, userName, roles, null, null, null, null)
        {
        }

        /// <summary>
        /// Constructor with all available parameters
        /// </summary>
        /// <param name="controller">Contoller you want to create context for</param>
        /// <param name="userName">Identity user name</param>
        /// <param name="roles">Roles for context<</param>
        /// <param name="formParams">Form params for context</param>
        /// <param name="queryStringParams">Params from query string for context</param>
        /// <param name="cookies">Cookies values</param>
        /// <param name="sessionItems">Session values</param>
        public FakeControllerContext
            (
                ControllerBase controller,
                string userName,
                string[] roles,
                NameValueCollection formParams,
                NameValueCollection queryStringParams,
                HttpCookieCollection cookies,
                SessionStateItemCollection sessionItems
            )
            : base(new FakeHttpContext(new FakePrincipal(new FakeIdentity(userName), roles), formParams, queryStringParams, cookies, sessionItems), new RouteData(), controller)
        { }
    }
}
