using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace CarRental.Tests.WEB.Fakes
{
    public class FakeHttpContext : HttpContextBase
    {
        private readonly FakePrincipal _principal;
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;
        private readonly SessionStateItemCollection _sessionItems;

        public FakeHttpContext(FakePrincipal principal, NameValueCollection formParams,
            NameValueCollection queryStringParams, HttpCookieCollection cookies, SessionStateItemCollection sessionItems)
        {
            _principal = principal;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            _sessionItems = sessionItems;
        }

        public override HttpRequestBase Request => new FakeHttpRequest(_formParams, _queryStringParams, _cookies);

        public override IPrincipal User
        {
            get { return _principal; }
            set { throw new NotImplementedException(); }
        }

        public override HttpSessionStateBase Session => new FakeHttpSessionState(_sessionItems);

        public override HttpServerUtilityBase Server => new FakeHttpServerUtility();
    }
}
