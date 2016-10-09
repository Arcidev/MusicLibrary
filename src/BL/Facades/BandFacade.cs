using AutoMapper;
using BL.DTO;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using DotVVM.Framework.Storage;
using System;
using System.Collections.Generic;

namespace BL.Facades
{
    public class BandFacade : BaseFacade
    {
        public Func<BandRepository> BandRepositoryFunc { get; set; }

        public Func<BandAlbumsQuery> BandAlbumsQueryFunc { get; set; }

        public StorageFileFacade StorageFileFacade { get; set; }

        public void AddBand(BandDTO band, UploadedFile file = null, IUploadedFileStorage storage = null)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var entity = Mapper.Map<Band>(band);

                if (file != null && storage != null)
                {
                    var fileName = StorageFileFacade.SaveFile(file, storage);
                    entity.ImageStorageFile = new StorageFile()
                    {
                        DisplayName = file.FileName,
                        FileName = fileName
                    };
                }

                var repo = BandRepositoryFunc();
                repo.Insert(entity);

                uow.Commit();
            }
        }

        public IList<AlbumDTO> GetBandAlbums(int bandId, int? excludeAlbumId = null, int? count = null, bool? approved = null)
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
    }
}
