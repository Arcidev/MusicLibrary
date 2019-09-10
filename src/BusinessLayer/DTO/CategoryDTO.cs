
using System.Threading;

namespace BusinessLayer.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Name_skSK { get; set; }

        public string Name_csCZ { get; set; }

        public string Name_esES { get; set; }

        public string NameLocalized
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.Name switch
                {
                    "sk-Sk" => Name_skSK ?? Name,
                    "cs-CZ" => Name_csCZ ?? Name,
                    "es-ES" => Name_esES ?? Name,
                    _ => Name,
                };
            }
        }
    }
}
