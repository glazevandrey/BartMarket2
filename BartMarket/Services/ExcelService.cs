using BartMarket.Template;

namespace BartMarket.Services
{
    public class ExcelService
    {
        public string OzonParse(IBaseOzonTemplate template, int count)
        {
            return template.Parse(count);
        }
    }
}
