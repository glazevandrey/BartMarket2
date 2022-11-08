using System.Threading.Tasks;

namespace BartMarket.Quartz
{
    public interface IQuartzService
    {
        Task MainParse();
    }
}
