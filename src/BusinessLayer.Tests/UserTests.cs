using BusinessLayer.DTO;
using BusinessLayer.Facades;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;
using Xunit;

namespace BL.Tests
{
    public class UserTests : TestBase
    {
        private static UserFacade UserFacade => services.GetRequiredService<UserFacade>();

        [Fact]
        public void TestCRUDOperations()
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

            userFacade.AddUser(user);

            var dto = userFacade.VerifyAndGetUser("test@mail.sk", "123");
            Assert.Equal("Test", dto.FirstName);

            dto.FirstName = "Test2";
            userFacade.EditUser(dto);

            dto = userFacade.VerifyAndGetUser("test@mail.sk", "123");
            Assert.Equal("Test2", dto.FirstName);
            userFacade.DeleteUser(dto.Id);
        }

        [Fact]
        public void TestNotExistingUserGet()
        {
            var userFacade = UserFacade;
            Assert.Null(userFacade.GetUserByEmail("notexisting@mail.sk"));
        }
    }
}
