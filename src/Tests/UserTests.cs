using BL.DTO;
using BL.Facades;
using DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Tests
{
    [TestClass]
    public class UserTests : TestBase
    {
        public UserFacade UserFacade { get { return Container.Resolve<UserFacade>(); } }

        [TestMethod]
        public void TestUserCreation()
        {
            var userFacade = UserFacade;
            var user = new UserDTO()
            {
                Email = "test@mail.sk",
                FirstName = "Test",
                LastName = "Test",
                Password = "123",
                UserRole = UserRole.Admin
            };

            userFacade.AddUser(user);
            userFacade.DeleteUser(userFacade.VerifyAndGetUser("test@mail.sk", "123").Id);
        }

        [TestMethod, ExpectedException(typeof(UIException))]
        public void TestNotExistingUserGet()
        {
            var userFacade = UserFacade;
            userFacade.GetUserByEmail("notexisting@mail.sk");
        }
    }
}
