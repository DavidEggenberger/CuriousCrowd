using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Hubs;
using Server.Services;
using Shared.Constants;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Workers
{
    public class BackgroundDataWorker : BackgroundService
    {
        private readonly IHubContext<NotificationHub> hubContext;
        private static int skipCount;
        private readonly IMapper mapper;
        private readonly IServiceProvider services;
        private readonly Random rand = new Random();
        public BackgroundDataWorker(IHubContext<NotificationHub> hubContext, IMapper mapper, IServiceProvider services)
        {
            this.hubContext = hubContext;
            this.mapper = mapper;
            this.services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(rand.Next(1000));

                    var messageBundle = new MessageBundleDTO { Messages = new List<MessageDTO>() };

                    using (var scope = services.CreateScope())
                    {
                        var messageStreamingService =
                            scope.ServiceProvider
                                .GetRequiredService<MessageStreamingService>();

                        await foreach (var item in messageStreamingService.ReadMessages(Math.Min(1000, skipCount)))
                        {
                            messageBundle.Messages.Add(mapper.Map<MessageDTO>(item));
                        }
                        skipCount += 20;
                    }

                    await hubContext.Clients.All.SendAsync(SignalRConstants.NewMessages, messageBundle);
                }
            }
            catch(Exception ex)
            {

            }     
        }
    }
}
