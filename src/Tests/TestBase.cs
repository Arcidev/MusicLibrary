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
        public IWindsorContainer Container { get; private set; }

        [TestInitialize]
        public void TestsInit()
        {
            Container = new WindsorContainer()
                .AddFacility<TypedFactoryFacility>()
                .Install(FromAssembly.InThisApplication())
                .Install(new AutoMapperInstaller())
                .Install(new ServicesInstaller())
                .Install(new FacadesInstaller());
        }
    }
}
