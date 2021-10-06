using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EOTools.Translation
{
    public class EquipData
    {
        public string NameJP { get; set; }

        public string NameEN { get; set; }

        public EquipData()
        {
        }

        public EquipData(string _nameJP, string _nameEN)
        {
            NameJP = _nameJP;
            NameEN = _nameEN;
        }

    }
}
