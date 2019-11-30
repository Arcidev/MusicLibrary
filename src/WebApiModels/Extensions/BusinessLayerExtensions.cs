using BusinessLayer.DTO;
using System.Collections.Generic;
using System.Linq;

namespace WebApiModels.Extensions
{
    public static class BusinessLayerExtensions
    {
        public static AlbumViewModel ToAlbumViewModel(this AlbumDTO album)
        {
            return new AlbumViewModel()
            {
                BandId = album.BandId,
                CategoryId = album.CategoryId,
                CreateDate = album.CreateDate,
                Id = album.Id,
                ImageStorageFileId = album.ImageStorageFileId,
                Name = album.Name
            };
        }

        public static IEnumerable<AlbumViewModel> ToAlbumViewModel(this IEnumerable<AlbumDTO> albums)
        {
            return albums.Select(ToAlbumViewModel);
        }

        public static SongViewModel ToSongViewModel(this SongDTO song)
        {
            return new SongViewModel()
            {
                AudioStorageFileId = song.AudioStorageFileId,
                CreateDate = song.CreateDate,
                Id = song.Id,
                Name = song.Name,
                YoutubeUrlParam = song.YoutubeUrlParam
            };
        }

        public static IEnumerable<SongViewModel> ToSongViewModel(this IEnumerable<SongDTO> songs)
        {
            return songs.Select(ToSongViewModel);
        }

        public static BandViewModel ToBandViewModel(this BandDTO band)
        {
            return new BandViewModel()
            {
                CreateDate = band.CreateDate,
                Description = band.Description,
                Id = band.Id,
                ImageStorageFileId = band.ImageStorageFileId,
                Name = band.Name
            };
        }

        public static IEnumerable<BandViewModel> ToBandViewModel(this IEnumerable<BandDTO> bands)
        {
            return bands.Select(ToBandViewModel);
        }

        public static CategoryViewModel ToCategoryViewModel(this CategoryDTO category)
        {
            return new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                Name_csCZ = category.Name_csCZ,
                Name_skSK = category.Name_skSK,
                Name_esES = category.Name_esES
            };
        }

        public static IEnumerable<CategoryViewModel> ToCategoryViewModel(this IEnumerable<CategoryDTO> categories)
        {
            return categories.Select(ToCategoryViewModel);
        }
    }
}
