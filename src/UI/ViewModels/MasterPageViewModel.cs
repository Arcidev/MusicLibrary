using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNet.Identity;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels
{
    public abstract class MasterPageViewModel : DotvvmViewModelBase
    {
        public string UserId => Context.GetAuthentication().User.Identity.GetUserId();

        public string ActivePage { get; protected set; }

        public bool IsUserLoggedIn => Context.GetAuthentication().User.Identity.IsAuthenticated;

        public string SearchString { get; set; }

        public void SignOut()
        {
            Context.GetAuthentication().SignOut();
            Context.RedirectToRoute("Index");
        }

        protected bool ExecuteSafely(Action action, Action successCallback = null, Action<UIException> failureCallback = null, Action<Exception> totalFailureCallback = null)
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
