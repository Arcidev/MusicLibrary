using BL.DTO;
using BL.Facades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Enums;
using System.Threading.Tasks;

namespace BL.Tests
{
    [TestClass]
    public class UserTests : TestBase
    {
        public UserFacade UserFacade => Container.Resolve<UserFacade>();

        [TestMethod]
        public async Task TestCRUDOperations()
        {
            var userFacade = UserFacade;
            var user = new UserCreateDTO()
            {
                Email = "test@mail.sk",
                FirstName = "Test",
                LastName = "Test",
                Password = "123",
                UserRole = UserRole.Admin
            };

            await userFacade.AddUserAsync(user);

            var dto = await userFacade.VerifyAndGetUserAsync("test@mail.sk", "123");
            Assert.AreEqual("Test", dto.FirstName);

            dto.FirstName = "Test2";
            userFacade.EditUser(dto);

            dto = await userFacade.VerifyAndGetUserAsync("test@mail.sk", "123");
            Assert.AreEqual("Test2", dto.FirstName);
            userFacade.DeleteUser(dto.Id);
        }

        [TestMethod]
        public async Task TestNotExistingUserGet()
        {
            var userFacade = UserFacade;
            Assert.IsNull(await userFacade.GetUserByEmailAsync("notexisting@mail.sk"));
        }
    }
}
