using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
	public class MasterPageViewModel : DotvvmViewModelBase
	{
        public bool IsUserLoggedIn { get { return Context.OwinContext.Authentication.User.Identity.IsAuthenticated; } }

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
