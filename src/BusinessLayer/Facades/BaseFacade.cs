using DotVVM.Framework.Controls;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace BusinessLayer.Facades
{
    public abstract class BaseFacade
    {
        protected readonly Func<IUnitOfWorkProvider> uowProviderFunc;

        protected BaseFacade(Func<IUnitOfWorkProvider> uowProviderFunc)
        {
            this.uowProviderFunc = uowProviderFunc;
        }

        protected void IsNotNull(object obj, string errorMessage)
        {
            if (obj == null)
                throw new UIException(errorMessage);
        }

        protected static async Task FillDataSetAsync<T>(GridViewDataSet<T> dataSet, IQuery<T> query)
        {
            query.Skip = dataSet.PagingOptions.PageIndex * dataSet.PagingOptions.PageSize;
            query.Take = dataSet.PagingOptions.PageSize;

            query.ClearSortCriteria();
            query.AddSortCriteria(dataSet.SortingOptions.SortExpression, dataSet.SortingOptions.SortDescending ? SortDirection.Descending : SortDirection.Ascending);

            dataSet.PagingOptions.TotalItemsCount = await query.GetTotalRowCountAsync();
            dataSet.Items = await query.ExecuteAsync();
        }
    }
}
