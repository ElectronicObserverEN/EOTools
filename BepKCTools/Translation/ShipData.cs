namespace EOTools.Translation
{
    public class ShipData
    {
        public string NameJP { get; set; }

        public string NameEN { get; set; }

        public ShipData()
        {
        }

        public ShipData(string _nameJP, string _nameEN)
        {
            NameJP = _nameJP;
            NameEN = _nameEN;
        }

    }
}
