using System.Collections.Specialized;
using System.Web;

namespace CarRental.Tests.WEB.Fakes
{
    public class FakeHttpRequest : HttpRequestBase
    {
        public FakeHttpRequest(NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies)
        {
            Form = formParams;
            QueryString = queryStringParams;
            Cookies = cookies;
        }

        public override NameValueCollection Form { get; }

        public override NameValueCollection QueryString { get; }

        public override HttpCookieCollection Cookies { get; }
    }
}
