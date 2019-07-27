using AutoMapper;
using DotVVM.Framework.Controls;
using Riganti.Utils.Infrastructure.Core;
using System;

namespace BusinessLayer.Facades
{
    public abstract class BaseFacade
    {
        protected readonly IMapper mapper;
        protected readonly Func<IUnitOfWorkProvider> uowProviderFunc;

        protected BaseFacade(IMapper mapper, Func<IUnitOfWorkProvider> uowProviderFunc)
        {
            this.mapper = mapper;
            this.uowProviderFunc = uowProviderFunc;
        }

        protected void IsNotNull(object obj, string errorMessage)
        {
            if (obj == null)
                throw new UIException(errorMessage);
        }

        protected void FillDataSet<T>(GridViewDataSet<T> dataSet, IQuery<T> query)
        {
            query.Skip = dataSet.PagingOptions.PageIndex * dataSet.PagingOptions.PageSize;
            query.Take = dataSet.PagingOptions.PageSize;

            query.ClearSortCriteria();
            query.AddSortCriteria(dataSet.SortingOptions.SortExpression, dataSet.SortingOptions.SortDescending ? SortDirection.Descending : SortDirection.Ascending);

            dataSet.PagingOptions.TotalItemsCount = query.GetTotalRowCount();
            dataSet.Items = query.Execute();
        }
    }
}
