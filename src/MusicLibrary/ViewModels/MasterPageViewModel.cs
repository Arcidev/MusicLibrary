using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
    public abstract class MasterPageViewModel : DotvvmViewModelBase
    {
        public int UserId => int.TryParse(Context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) ? userId : 0;

        public string ActivePage { get; protected set; }

        public bool IsUserLoggedIn => Context.HttpContext.User.Identity.IsAuthenticated;

        public string SearchString { get; set; }

        public async Task SignOut()
        {
            await Context.GetAuthentication().SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Context.RedirectToRoute("Index");
        }

        protected async Task<bool> ExecuteSafelyAsync(Func<Task> action, Action successCallback = null, Action<UIException> failureCallback = null, Action<Exception> totalFailureCallback = null)
        {
            try
            {
                await action();
                successCallback?.Invoke();
                return true;
            }
            catch (UIException ex)
            {
                failureCallback?.Invoke(ex);
            }
            catch (Exception ex)
            {
                totalFailureCallback?.Invoke(ex);
            }
            return false;
        }
    }
}
