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
        public UserFacade UserFacade { get { return Container.Resolve<UserFacade>(); } }

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
            dto.Email = "test2@mail.sk";
            userFacade.EditUser(dto);

            await userFacade.VerifyAndGetUserAsync("test2@mail.sk", "123");
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
