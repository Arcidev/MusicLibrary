using BL.Configuration;
using BL.Installers;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BL.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        public WindsorContainer Container { get; set; }

        [TestInitialize]
        public void TestsInit()
        {
            AutoMapper.Init();

            Container = new WindsorContainer();
            Container.AddFacility<TypedFactoryFacility>();
            Container.Install(FromAssembly.InThisApplication());
            Container.Install(new FacadesInstaller());
            Container.Install(new ServicesInstaller());
        }
    }
}
