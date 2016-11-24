using AutoMapper;
using BL.DTO;
using BL.Queries;
using BL.Repositories;
using BL.Resources;
using DAL.Entities;
using DotVVM.Framework.Storage;
using System;
using System.Collections.Generic;

namespace BL.Facades
{
    public class BandFacade : ImageStorableFacade
    {
        public Func<BandRepository> BandRepositoryFunc { get; set; }

        public Func<BandsQuery> BandsQueryFunc { get; set; }

        public Func<BandAlbumsQuery> BandAlbumsQueryFunc { get; set; }

        public BandDTO AddBand(BandCreateDTO band, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<Band>(band);
                entity.CreateDate = DateTime.Now;
                SetImageFile(entity, file, storage);

                var repo = BandRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
                return Mapper.Map<BandDTO>(entity);
            }
        }

        public IEnumerable<AlbumDTO> GetBandAlbums(int bandId, int? excludeAlbumId = null, int? count = null, bool? approved = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = BandAlbumsQueryFunc();
                query.BandId = bandId;
                query.ExcludeAlbumId = excludeAlbumId;
                query.Approved = approved;
                query.Take = count;

                return query.Execute();
            }
        }

        public IEnumerable<BandDTO> GetBands()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = BandsQueryFunc();
                return query.Execute();
            }
        }

        public BandDTO GetBand(int id)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = BandRepositoryFunc();
                var entity = repo.GetById(id);
                IsNotNull(entity, ErrorMessages.BandNotExist);

                return Mapper.Map<BandDTO>(entity);
            }
        }
    }
}
