
using System.Threading;

namespace BL.DTO
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
                switch(Thread.CurrentThread.CurrentCulture.Name)
                {
                    case "sk-Sk":
                        return Name_skSK ?? Name;
                    case "cs-CZ":
                        return Name_csCZ ?? Name;
                    case "es-ES":
                        return Name_esES ?? Name;
                    default:
                        return Name;
                }
            }
        }
    }
}
