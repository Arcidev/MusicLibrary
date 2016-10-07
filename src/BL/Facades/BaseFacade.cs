using Riganti.Utils.Infrastructure.Core;
using System;

namespace BL.Facades
{
    public class BaseFacade
    {
        public Func<IUnitOfWorkProvider> UowProviderFunc { get; set; }

        public void IsNotNull(object obj, string errorMessage)
        {
            if (obj == null)
                throw new UIException(errorMessage);
        }
    }
}
