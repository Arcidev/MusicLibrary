using AutoMapper;
using BusinessLayer.DTO;
using BusinessLayer.Queries;
using BusinessLayer.Repositories;
using BusinessLayer.Resources;
using DataLayer.Entities;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Storage;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace BusinessLayer.Facades
{
    public class UserFacade : ImageStorableFacade
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 160 / 8;
        private const int saltSize = 128 / 8;

        private readonly Func<UserRepository> userRepositoryFunc;
        private readonly Func<UsersQuery> usersQueryFunc;

        public UserFacade(Func<UserRepository> userRepositoryFunc,
            Func<UsersQuery> usersQueryFunc,
            IMapper mapper,
            Lazy<StorageFileFacade> storageFileFacade
            , Func<IUnitOfWorkProvider> uowProviderFunc) : base(mapper, storageFileFacade, uowProviderFunc)
        {
            this.userRepositoryFunc = userRepositoryFunc;
            this.usersQueryFunc = usersQueryFunc;
        }

        public UserDTO AddUser(UserCreateDTO user)
        {
            using var uow = uowProviderFunc().Create();
            if (GetUserByEmail(user.Email) != null)
                throw new UIException(ErrorMessages.EmailAlreadyUsed);

            var entity = mapper.Map<User>(user);
            var (hash, salt) = CreateHash(user.Password);
            entity.PasswordHash = hash;
            entity.PasswordSalt = salt;

            var repo = userRepositoryFunc();
            repo.Insert(entity);

            uow.Commit();

            return mapper.Map<UserDTO>(entity);
        }

        public UserDTO VerifyAndGetUser(string email, string password)
        {
            var user = GetUserByEmail(email);
            IsNotNull(user, ErrorMessages.VerificationFailed);

            if (VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, password))
                return mapper.Map<UserDTO>(user);

            throw new UIException(ErrorMessages.VerificationFailed);
        }

        public UserDTO GetUserByEmail(string email)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userRepositoryFunc();
            var user = repo.GetByEmail(email);

            return user != null ? mapper.Map<UserDTO>(user) : null;
        }

        public UserDTO GetUser(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userRepositoryFunc();
            var user = repo.GetById(id);
            IsNotNull(user, ErrorMessages.UserNotExist);

            return mapper.Map<UserDTO>(user);
        }

        public void EditUser(UserEditDTO user, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userRepositoryFunc();
            var entity = repo.GetById(user.Id) ?? throw new UIException(ErrorMessages.UserNotExist);
            SetImageFile(entity, file, storage);

            mapper.Map(user, entity);
            if (!string.IsNullOrEmpty(user.Password))
            {
                var (hash, salt) = CreateHash(user.Password);
                entity.PasswordHash = hash;
                entity.PasswordSalt = salt;
            }

            uow.Commit();
        }

        public void DeleteUser(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userRepositoryFunc();
            repo.Delete(id);

            uow.Commit();
        }

        public void LoadUserInfoes(GridViewDataSet<UserInfoDTO> dataSet, string filter)
        {
            using var uow = uowProviderFunc().Create();
            var query = usersQueryFunc();
            query.Filter = filter;

            FillDataSet(dataSet, query);
        }

        private (string hash, string salt) CreateHash(string password)
        {
            using var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, PBKDF2IterCount);
            var salt = deriveBytes.Salt;
            var subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);

            return (Convert.ToBase64String(subkey), Convert.ToBase64String(salt));
        }

        private bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            var saltBytes = Convert.FromBase64String(salt);

            using var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount);
            var generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            return hashedPasswordBytes.SequenceEqual(generatedSubkey);
        }
    }
}
