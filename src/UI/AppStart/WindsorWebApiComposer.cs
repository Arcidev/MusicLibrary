using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Castle.Windsor;

namespace MusicLibrary.AppStart
{
    public class WindsorWebApiComposer : IHttpControllerActivator
    {
        private readonly IWindsorContainer container;

        public WindsorWebApiComposer(IWindsorContainer container)
        {
            this.container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = (IHttpController)container.Resolve(controllerType);
            request.RegisterForDispose(new Release(() => container.Release(controller)));
            return controller;
        }

        private class Release : IDisposable
        {
            private readonly Action release;

            public Release(Action release) { this.release = release; }

            public void Dispose()
            {
                release();
            }
        }
    }
}
