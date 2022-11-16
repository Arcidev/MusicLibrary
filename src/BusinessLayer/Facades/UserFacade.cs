using BusinessLayer.DTO;
using BusinessLayer.Queries;
using BusinessLayer.Repositories;
using BusinessLayer.Resources;
using DataLayer.Entities;
using DotVVM.Core.Storage;
using DotVVM.Framework.Controls;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BusinessLayer.Facades
{
    public class UserFacade : ImageStorableFacade
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 512 / 8;
        private const int saltSize = 256 / 8;

        private readonly Func<UserRepository> userRepositoryFunc;
        private readonly Func<UsersQuery> usersQueryFunc;

        public UserFacade(Func<UserRepository> userRepositoryFunc,
            Func<UsersQuery> usersQueryFunc,
            Lazy<StorageFileFacade> storageFileFacade,
            Func<IUnitOfWorkProvider> uowProviderFunc) : base(storageFileFacade, uowProviderFunc)
        {
            this.userRepositoryFunc = userRepositoryFunc;
            this.usersQueryFunc = usersQueryFunc;
        }

        public async Task<UserDTO> AddUserAsync(UserCreateDTO user)
        {
            if (await GetUserByEmailAsync(user.Email) != null)
                throw new UIException(ErrorMessages.EmailAlreadyUsed);

            using var uow = uowProviderFunc().Create();
            var entity = user.Adapt<User>();
            var (hash, salt) = CreateHash(user.Password);
            entity.PasswordHash = hash;
            entity.PasswordSalt = salt;

            var repo = userRepositoryFunc();
            repo.Insert(entity);

            await uow.CommitAsync();

            return entity.Adapt<UserDTO>();
        }

        public async Task<UserDTO> VerifyAndGetUserAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            IsNotNull(user, ErrorMessages.VerificationFailed);

            if (VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, password))
                return user.Adapt<UserDTO>();

            throw new UIException(ErrorMessages.VerificationFailed);
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userRepositoryFunc();
            var user = await repo.GetByEmailAsync(email);

            return user?.Adapt<UserDTO>();
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userRepositoryFunc();
            var user = await repo.GetByIdAsync(id);
            IsNotNull(user, ErrorMessages.UserNotExist);

            return user.Adapt<UserDTO>();
        }

        public async Task EditUserAsync(UserEditDTO user, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userRepositoryFunc();
            var entity = await repo.GetByIdAsync(user.Id) ?? throw new UIException(ErrorMessages.UserNotExist);
            await SetImageFileAsync(entity, file, storage);

            user.Adapt(entity);
            if (!string.IsNullOrEmpty(user.Password))
            {
                var (hash, salt) = CreateHash(user.Password);
                entity.PasswordHash = hash;
                entity.PasswordSalt = salt;
            }

            await uow.CommitAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = userRepositoryFunc();
            repo.Delete(id);

            await uow.CommitAsync();
        }

        public async Task LoadUserInfoesAsync(GridViewDataSet<UserInfoDTO> dataSet, string filter)
        {
            using var uow = uowProviderFunc().Create();
            var query = usersQueryFunc();
            query.Filter = filter;

            await FillDataSetAsync(dataSet, query);
        }

        private static (string hash, string salt) CreateHash(string password)
        {
            using var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, PBKDF2IterCount, HashAlgorithmName.SHA512);
            var salt = deriveBytes.Salt;
            var subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);

            return (Convert.ToBase64String(subkey), Convert.ToBase64String(salt));
        }

        private static bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            var saltBytes = Convert.FromBase64String(salt);

            using var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount, HashAlgorithmName.SHA512);
            var generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            return hashedPasswordBytes.SequenceEqual(generatedSubkey);
        }
    }
}
