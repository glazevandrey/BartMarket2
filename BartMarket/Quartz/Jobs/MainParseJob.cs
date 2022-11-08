using Quartz;
using System.Threading.Tasks;

namespace BartMarket.Quartz.Jobs
{
    public class MainParseJob : IJob
    {
        private readonly IQuartzService _quartzService;

        public MainParseJob(IQuartzService quartzService)
        {
            _quartzService = quartzService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _quartzService.MainParse();
        }
    }
}
