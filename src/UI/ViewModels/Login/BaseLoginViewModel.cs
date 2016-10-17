using BL.DTO;
using MusicLibrary.Identity;
using System.Security.Claims;

namespace MusicLibrary.ViewModels.Login
{
	public class BaseLoginViewModel : MasterPageViewModel
	{
        public void SignUserIn(UserDTO user)
        {
            var claimsIdentity = new ClaimsIdentity(new UserIdentity(user.FullName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.UserRole.ToString()));
            Context.OwinContext.Authentication.SignIn(claimsIdentity);
        }
	}
}
