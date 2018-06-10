using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace WebJob{
    public class Jobs{
        public async Task WaitForTimer([TimerTrigger("*/5 * * * * *", RunOnStartup = true)]
            TimerInfo timerInfo, TextWriter log) {
            await log.WriteAsync($"C# Timer trigger function executed at: {DateTime.Now} ");
        }
    }
}