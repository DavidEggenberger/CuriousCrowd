using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.BuildingBlocks;
using Server.Data;
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
        private readonly IMapper mapper;
        private readonly IServiceProvider services;
        private readonly Random rand = new Random();
        private readonly SkipCountProvider skipCountProvider;
        private readonly DataContextContainer dataContextContainer;
        public BackgroundDataWorker(IHubContext<NotificationHub> hubContext, IMapper mapper, IServiceProvider services, DataContextContainer dataContextContainer, SkipCountProvider skipCountProvider)
        {
            this.hubContext = hubContext;
            this.mapper = mapper;
            this.services = services;
            this.skipCountProvider = skipCountProvider;
            this.dataContextContainer = dataContextContainer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (true)
                {
                    await Task.Delay(new Random().Next(0, 2500));

                    var messageBundle = new MessageBundleDTO { Messages = new List<MessageDTO>() };
                    var messages = new List<Message>();

                    using (var scope = services.CreateScope())
                    {
                        var messageStreamingService =
                            scope.ServiceProvider
                                .GetRequiredService<MessageStreamingService>();

                        await foreach (var item in messageStreamingService.ReadMessages(skipCountProvider.GetSkipCount()))
                        {
                            messageBundle.Messages.Add(mapper.Map<MessageDTO>(item));
                            messages.Add(item);
                        }
                        skipCountProvider.Increment();
                    }

                    dataContextContainer.AddMessages(messages);

                    await hubContext.Clients.All.SendAsync(SignalRConstants.NewMessages, new MessageBundleDTO { Messages = messageBundle.Messages.DistinctBy(m => m.Id).ToList() });
                }
            }
            catch(Exception ex)
            {

            }     
        }
    }
}
