using DotVVM.Framework.Controls;
using Riganti.Utils.Infrastructure.Core;
using System;

namespace BL.Facades
{
    public abstract class BaseFacade
    {
        public Func<IUnitOfWorkProvider> UowProviderFunc { get; set; }

        protected void IsNotNull(object obj, string errorMessage)
        {
            if (obj == null)
                throw new UIException(errorMessage);
        }

        protected void FillDataSet<T>(GridViewDataSet<T> dataSet, IQuery<T> query)
        {
            query.Skip = dataSet.PagingOptions.PageIndex * dataSet.PagingOptions.PageSize;
            query.Take = dataSet.PagingOptions.PageSize;

            query.SortCriteria.Clear();
            query.AddSortCriteria(dataSet.SortingOptions.SortExpression, dataSet.SortingOptions.SortDescending ? SortDirection.Descending : SortDirection.Ascending);

            dataSet.PagingOptions.TotalItemsCount = query.GetTotalRowCount();
            dataSet.Items = query.Execute();
        }
    }
}
