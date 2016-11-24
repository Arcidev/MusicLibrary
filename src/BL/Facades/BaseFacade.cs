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
    }
}
