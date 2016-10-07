using AutoMapper;
using BL.DTO;
using BL.Repositories;
using BL.Resources;
using DAL.Entities;
using DotVVM.Framework.Storage;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BL.Facades
{
    public class UserFacade : BaseFacade
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 160 / 8;
        private const int saltSize = 128 / 8;

        public Func<UserRepository> UserRepositoryFunc { get; set; }

        public StorageFileFacade StorageFileFacade { get; set; }

        public void AddUser(UserDTO user, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<User>(user);
                var password = CreateHash(user.Password);
                entity.PasswordHash = password.Item1;
                entity.PasswordSalt = password.Item2;

                if (file != null && storage != null)
                {
                    var fileName = StorageFileFacade.SaveFile(file, storage);
                    entity.ImageStorageFile = new StorageFile()
                    {
                        DisplayName = file.FileName,
                        FileName = fileName
                    };
                }

                var repo = UserRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
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

        public UserDTO VerifyAndGetUser(string email, string password)
        {
            var user = GetUserByEmail(email);
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
                IsNotNull(user, ErrorMessages.UserNotExist);

                return Mapper.Map<UserDTO>(user);
            }
        }

        public UserDTO GetUserByEmail(string email)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = UserRepositoryFunc();
                var user = repo.GetByEmail(email);
                IsNotNull(user, ErrorMessages.UserNotExist);

                return Mapper.Map<UserDTO>(user);
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

        private Tuple<string, string> CreateHash(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, PBKDF2IterCount))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);

                return Tuple.Create(Convert.ToBase64String(subkey), Convert.ToBase64String(salt));
            }
        }

        private bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount))
            {
                byte[] generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
                return hashedPasswordBytes.SequenceEqual(generatedSubkey);
            }
        }
    }
}
