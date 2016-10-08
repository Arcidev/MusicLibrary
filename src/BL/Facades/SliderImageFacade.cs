using BL.DTO;
using BL.Queries;
using System;
using System.Collections.Generic;

namespace BL.Facades
{
    public class SliderImageFacade : BaseFacade
    {
        public Func<SliderImagesQuery> SliderImageQueryFunc { get; set; }

        public IList<SliderImageDTO> GetImages()
        {
            using (var uow = UowProviderFunc().Create())
            {
                var query = SliderImageQueryFunc();
                return query.Execute();
            }
        }
    }
}
