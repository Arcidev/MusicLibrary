using AutoMapper;
using BL.DTO;
using BL.Queries;
using BL.Repositories;
using BL.Resources;
using DAL.Entities;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Storage;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BL.Facades
{
    public class UserFacade : ImageStorableFacade
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 160 / 8;
        private const int saltSize = 128 / 8;

        public Func<UserRepository> UserRepositoryFunc { get; set; }

        public Func<UsersQuery> UsersQueryFunc { get; set; }

        public IMapper Mapper { get; set; }

        public async Task<UserDTO> AddUserAsync(UserCreateDTO user)
        {
            using (var uow = UowProviderFunc().Create())
            {
                if (await GetUserByEmailAsync(user.Email) != null)
                    throw new UIException(ErrorMessages.EmailAlreadyUsed);

                var entity = Mapper.Map<User>(user);
                var (hash, salt) = CreateHash(user.Password);
                entity.PasswordHash = hash;
                entity.PasswordSalt = salt;

                var repo = UserRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();

                return Mapper.Map<UserDTO>(entity);
            }
        }

        public async Task<UserDTO> VerifyAndGetUserAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            IsNotNull(user, ErrorMessages.VerificationFailed);

            if (VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, password))
                return Mapper.Map<UserDTO>(user);

            throw new UIException(ErrorMessages.VerificationFailed);
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = UserRepositoryFunc();
                var user = await repo.GetByEmailAsync(email);

                return user != null ? Mapper.Map<UserDTO>(user) : null;
            }
        }

        public UserDTO GetUser(int id)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = UserRepositoryFunc();
                var user = repo.GetById(id);
                IsNotNull(user, ErrorMessages.UserNotExist);

                return Mapper.Map<UserDTO>(user);
            }
        }

        public void EditUser(UserEditDTO user, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = UserRepositoryFunc();
                var entity = repo.GetById(user.Id) ?? throw new UIException(ErrorMessages.UserNotExist);
                SetImageFile(entity, file, storage);
                
                Mapper.Map(user, entity);
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var (hash, salt) = CreateHash(user.Password);
                    entity.PasswordHash = hash;
                    entity.PasswordSalt = salt;
                }

                uow.Commit();
            }
        }

        public void DeleteUser(int id)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = UserRepositoryFunc();
                repo.Delete(id);

                uow.Commit();
            }
        }

        public void LoadUserInfoes(GridViewDataSet<UserInfoDTO> dataSet, string filter)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = UsersQueryFunc();
                query.Filter = filter;

                FillDataSet(dataSet, query);
            }
        }

        private (string hash, string salt) CreateHash(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, PBKDF2IterCount))
            {
                var salt = deriveBytes.Salt;
                var subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);

                return (Convert.ToBase64String(subkey), Convert.ToBase64String(salt));
            }
        }

        private bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            var saltBytes = Convert.FromBase64String(salt);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount))
            {
                var generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
                return hashedPasswordBytes.SequenceEqual(generatedSubkey);
            }
        }
    }
}
