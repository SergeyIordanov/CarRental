using System.Linq;
using System.Security.Principal;

namespace CarRental.Tests.WEB.Fakes
{
    public class FakePrincipal : IPrincipal
    {
        private readonly string[] _roles;

        public FakePrincipal(IIdentity identity, string[] roles)
        {
            Identity = identity;
            _roles = roles;
        }

        public IIdentity Identity { get; }

        public bool IsInRole(string role)
        {
            if (_roles == null)
                return false;
            return _roles.Contains(role);
        }
    }
}
