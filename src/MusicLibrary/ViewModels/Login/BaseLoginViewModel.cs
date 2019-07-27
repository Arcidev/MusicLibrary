using BusinessLayer.DTO;
using BusinessLayer.Facades;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Login
{
    public class BaseLoginViewModel : MasterPageViewModel
    {
        protected readonly UserFacade userFacade;

        [Bind(Direction.ServerToClient)]
        public string ErrorMessage { get; set; }

        public BaseLoginViewModel(UserFacade userFacade)
        {
            this.userFacade = userFacade;
        }

        protected async Task SignUserIn(UserDTO user)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                new Claim(ClaimTypes.AuthenticationMethod, CookieAuthenticationDefaults.AuthenticationScheme)
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = true ? DateTime.UtcNow.AddMonths(1) : (DateTime?)null
            };

            await Context.GetAuthentication().SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
        }
    }
}
