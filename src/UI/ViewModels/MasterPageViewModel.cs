using DotVVM.Framework.ViewModel;
using Microsoft.AspNet.Identity;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
	public abstract class MasterPageViewModel : DotvvmViewModelBase
	{
        protected string UserId { get { return Context.OwinContext.Authentication.User.Identity.GetUserId(); } }

        public string ActivePage { get; protected set; }

        public bool IsUserLoggedIn { get { return Context.OwinContext.Authentication.User.Identity.IsAuthenticated; } }

        public string SearchString { get; set; }

        public void SignOut()
        {
            Context.OwinContext.Authentication.SignOut();
            Context.RedirectToRoute("Index");
        }

        public bool ExecuteSafely(Action action, Action successCallback = null, Action<UIException> failureCallback = null, Action<Exception> totalFailureCallback = null)
        {
            try
            {
                action();
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

        public async Task<bool> ExecuteSafelyAsync(Func<Task> action, Action successCallback = null, Action<UIException> failureCallback = null, Action<Exception> totalFailureCallback = null)
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
