﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class SongsQuery<T> : AppQuery<T>
    {
        public SongsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        public string Filter { get; set; }

        public bool? Approved { get; set; }

        protected override IQueryable<T> GetQueryable()
        {
            var query = Context.Songs.AsQueryable();

            if (!string.IsNullOrEmpty(Filter))
                query = query.Where(x => x.Name.Contains(Filter));

            if (Approved.HasValue)
                query = query.Where(x => x.Approved == Approved.Value);

            return query.ProjectTo<T>(mapperConfig);
        }
    }
}
