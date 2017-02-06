using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using MusicLibrary.Identity;
using System.Security.Claims;

namespace MusicLibrary.ViewModels.Login
{
	public class BaseLoginViewModel : MasterPageViewModel
	{
        [Bind(Direction.None)]
        public UserFacade UserFacade { get; set; }

        [Bind(Direction.ServerToClient)]
        public string ErrorMessage { get; set; }

        protected void SignUserIn(UserDTO user)
        {
            var claimsIdentity = new ClaimsIdentity(new UserIdentity(user.FullName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.UserRole.ToString()));
            Context.GetAuthentication().SignIn(claimsIdentity);
        }
	}
}
