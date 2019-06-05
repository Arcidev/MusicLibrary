using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace MusicLibrary.Identity
{
    public class UserIdentity : IIdentity
    {
        public string AuthenticationType => DefaultAuthenticationTypes.ApplicationCookie;

        public bool IsAuthenticated { get; set; }

        public string Name { get; private set; }

        public UserIdentity(string name)
        {
            Name = name;
        }
    }
}