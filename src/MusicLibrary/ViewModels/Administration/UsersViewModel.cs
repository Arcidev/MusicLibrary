using BusinessLayer.DTO;
using BusinessLayer.Facades;
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
    [Authorize(Roles = nameof(Shared.Enums.UserRole.Admin))]
    public class UsersViewModel : AdministrationMasterPageViewModel
    {
        private readonly UserFacade userFacade;

        public GridViewDataSet<UserInfoDTO> Users { get; set; }

        public IEnumerable<string> UserRoles { get; set; }

        public string Filter { get; set; }

        public string SelectedUserRole { get; set; }

        [Bind(Direction.ServerToClient)]
        public string ErrorMessage { get; set; }

        public UsersViewModel(UserFacade userFacade)
        {
            this.userFacade = userFacade;

            UserRoles = Enum.GetNames(typeof(UserRole));
            Users = new GridViewDataSet<UserInfoDTO>()
            {
                PagingOptions = new PagingOptions()
                {
                    PageSize = 20
                },
                SortingOptions = new SortingOptions()
                {
                    SortExpression = nameof(UserInfoDTO.Email)
                },
                RowEditOptions = new RowEditOptions()
                {
                    PrimaryKeyPropertyName = nameof(UserInfoDTO.Id)
                }
            };
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ActiveAdminPage = "Users";
            }

            userFacade.LoadUserInfoes(Users, Filter);

            await base.PreRender();
        }

        public void Edit(UserInfoDTO user)
        {
            Users.RowEditOptions.EditRowId = user.Id;
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
                userFacade.EditUser(new UserEditDTO()
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
            Users.RowEditOptions.EditRowId = null;
            SelectedUserRole = null;
        }
    }
}
