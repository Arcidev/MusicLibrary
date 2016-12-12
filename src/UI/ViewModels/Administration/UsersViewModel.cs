using BL.DTO;
using BL.Facades;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using MusicLibrary.Resources;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibrary.ViewModels.Administration
{
    [Authorize(Roles = new[] { nameof(Shared.Enums.UserRole.Admin) })]
    public class UsersViewModel : AdministrationMasterPageViewModel
    {
        [Bind(Direction.None)]
        public UserFacade UserFacade { get; set; }

        public GridViewDataSet<UserInfoDTO> Users { get; set; }

        public IEnumerable<string> UserRoles { get; set; }

        public string Filter { get; set; }

        public string SelectedUserRole { get; set; }

        [Bind(Direction.ServerToClient)]
        public string ErrorMessage { get; set; }

        public UsersViewModel()
        {
            UserRoles = Enum.GetNames(typeof(UserRole));
            Users = new GridViewDataSet<UserInfoDTO>() { PageSize = 20, SortExpression = nameof(UserInfoDTO.Email), PrimaryKeyPropertyName = nameof(UserInfoDTO.Id) };
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "Users";
            }

            UserFacade.LoadUserInfoes(Users, Filter);

            await base.PreRender();
        }

        public void Edit(UserInfoDTO user)
        {
            Users.EditRowId = user.Id;
            SelectedUserRole = user.UserRole.ToString();
        }

        public void Update(UserInfoDTO user)
        {
            if (string.IsNullOrEmpty(user.FirstName))
                ErrorMessage = Texts.FirstNameRequired;

            if (string.IsNullOrEmpty(user.LastName))
                ErrorMessage = Texts.LastNameRequired;

            ExecuteSafely(() =>
            {
                UserFacade.EditUser(new UserEditDTO()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id,
                    UserRole = (UserRole)Enum.Parse(typeof(UserRole), SelectedUserRole)
                });

                CancelEdit();
            });
        }

        public void CancelEdit()
        {
            Users.EditRowId = null;
            SelectedUserRole = null;
        }
    }
}
