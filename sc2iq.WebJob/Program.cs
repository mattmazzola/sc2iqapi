﻿using Microsoft.Azure.WebJobs;

namespace sc2iq.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration();
            config.UseServiceBus();

            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
