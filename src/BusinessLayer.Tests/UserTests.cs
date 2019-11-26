using BusinessLayer.DTO;
using BusinessLayer.Facades;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;
using System.Threading.Tasks;
using Xunit;

namespace BL.Tests
{
    public class UserTests : TestBase
    {
        private static UserFacade UserFacade => services.GetRequiredService<UserFacade>();

        [Fact]
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
            Assert.Equal("Test", dto.FirstName);

            dto.FirstName = "Test2";
            await userFacade.EditUserAsync(dto);

            dto = await userFacade.VerifyAndGetUserAsync("test@mail.sk", "123");
            Assert.Equal("Test2", dto.FirstName);
            await userFacade.DeleteUserAsync(dto.Id);
        }

        [Fact]
        public async Task TestNotExistingUserGet()
        {
            var userFacade = UserFacade;
            Assert.Null(await userFacade.GetUserByEmailAsync("notexisting@mail.sk"));
        }
    }
}
