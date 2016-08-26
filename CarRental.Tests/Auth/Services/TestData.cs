using System.Collections;
using CarRental.Auth.BLL.DTO;
using NUnit.Framework;

namespace CarRental.Tests.Auth.Services
{
    public class TestData
    {

        public static IEnumerable WrongCreateUserTestCases
        {
            get
            {
                yield return new TestCaseData(new UserDTO());
                yield return new TestCaseData(new UserDTO { Email = "ok", Name = "ok" });
                yield return new TestCaseData(null);
                yield return new TestCaseData(new UserDTO { Email = "ok" });
                yield return new TestCaseData(new UserDTO { Password = "qweqwe123" });
                yield return new TestCaseData(new UserDTO { Email = "ok", Name = "ok", Password = "111", UserName = "ok", Role = "user"});
                yield return new TestCaseData(new UserDTO { Email = "ok", Name = "ok", Password = "qweqweqwe", UserName = "ok", Role = "user" });
                yield return new TestCaseData(new UserDTO { Email = "ok", Name = "ok", Password = "qwe1", UserName = "ok", Role = "user" });
            }
        }

        public static IEnumerable SuccessfulSetRoleTestCases
        {
            get
            {
                yield return new TestCaseData(new UserDTO { Id = "test", Email = "ok", Name = "ok" }, "test");
            }
        }

        public static IEnumerable WrongSetRoleTestCases
        {
            get
            {
                yield return new TestCaseData(new UserDTO { Id = "test", Name = "ok" }, "test");
                yield return new TestCaseData(null, "test");
                yield return new TestCaseData(null, null);
                yield return new TestCaseData(new UserDTO { Id = "test", Email = "ok", Name = "ok" }, null);
                yield return new TestCaseData(new UserDTO (), "test");
            }
        }
    }
}
