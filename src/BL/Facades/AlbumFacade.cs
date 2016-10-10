using AutoMapper;
using BL.DTO;
using BL.Queries;
using BL.Repositories;
using BL.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Facades
{
    public class AlbumFacade : BaseFacade
    {
        public Func<RecentAlbumsQuery> RecentlyAddedAlbumsQueryFunc { get; set; }

        public Func<FeaturedAlbumsQuery> FeaturedAlbumsQueryFunc { get; set; }

        public Func<AlbumRepository> AlbumRepositoryFunc { get; set; }

        public Func<AlbumSongsQuery> AlbumSongsQueryFunc { get; set; }

        public AlbumDTO GetAlbum(int id, bool includeBandInfo = true, bool includeSongs = true)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = AlbumRepositoryFunc();
                var entity = repo.GetById(id);
                IsNotNull(entity, ErrorMessages.AlbumNotExist);

                var dto = Mapper.Map<AlbumDTO>(entity);
                if (includeBandInfo)
                    dto.Band = Mapper.Map<BandDTO>(entity.Band);

                if (includeSongs)
                {
                    var query = AlbumSongsQueryFunc();
                    query.AlbumId = entity.Id;
                    query.Approved = true;
                    dto.Songs = query.Execute();
                }

                return dto;
            }
        }

        public IList<AlbumDTO> GetRecentAlbums(int count)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = RecentlyAddedAlbumsQueryFunc();
                query.Take = count;

                return query.Execute();
            }
        }

        public IList<AlbumDTO> GetFeaturedAlbums(int count)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = FeaturedAlbumsQueryFunc();
                query.Take = count;

                return query.Execute();
            }
        }
    }
}
