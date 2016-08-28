using System.Web;

namespace CarRental.Tests.WEB.Fakes
{
    public class FakeHttpServerUtility : HttpServerUtilityBase
    {
        public override string MapPath(string path)
        {
            return "";
        }
    }
}
