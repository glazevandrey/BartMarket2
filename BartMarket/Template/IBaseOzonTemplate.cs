using System.Collections.Generic;

namespace BartMarket.Template
{
    public interface IBaseOzonTemplate
    {
        public string Name { get; set; }
        public string PathToTemplate { get; set; }
        public List<string> KeyWords { get; set; }
        public List<Offer2> NeededOffers { get; set; }
        public string Parse(int count, bool ostatok);
        public string Prepare();

    }
}
