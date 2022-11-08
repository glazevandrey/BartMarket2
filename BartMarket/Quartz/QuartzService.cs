using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BartMarket.Quartz
{
    public class QuartzService : IQuartzService
    {
        public QuartzService()
        {
        }
        public async Task MainParse()
        {
            await Program.StartLite();
        }
    }
}
