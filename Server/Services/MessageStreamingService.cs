using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Server.BuildingBlocks;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class MessageStreamingService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly DateProvider dateProvider;
        private readonly DataContextContainer dataContextContainer;
        public MessageStreamingService(IWebHostEnvironment webHostEnvironment, DateProvider dateProvider, DataContextContainer dataContextContainer)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.dateProvider = dateProvider;
            this.dataContextContainer = dataContextContainer;
        }
        public async IAsyncEnumerable<Message> ReadMessages(int skip = 0)
        {
            using (var reader = new StreamReader($@"Data\{DataConstants.MessageCSVFilePath}"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    await foreach (var expandoObject in csv.GetRecordsAsync<dynamic>().Skip(skip).Take(10000))
                    {
                        var message = new Message();
                        try
                        {
                            var dictionary = (IDictionary<string, object>)expandoObject;

                            if (dictionary.TryGetValue("alliance_id", out var allianceId))
                            {
                                Alliance alliance = null;
                                if ((alliance = dataContextContainer.Alliances.SingleOrDefault(a => a.AllianceId == allianceId.ToString())) == null)
                                {
                                    continue;
                                }
                                message.Alliance = alliance;
                            }

                            if (double.TryParse(dictionary["FRAUD"].ToString(), out var fraud))
                            {
                                message.Fraud = fraud;
                            }

                            if (double.TryParse(dictionary["risk"].ToString(), out var risk))
                            {
                                message.Risk = risk;
                            }

                            if (double.TryParse(dictionary["VULGARITY"].ToString(), out var vulgarity))
                            {
                                message.Vulgarity = vulgarity;
                            }

                            if (double.TryParse(dictionary["VIOLENCE"].ToString(), out var violence))
                            {
                                message.Violence = violence;
                            }

                            if (double.TryParse(dictionary["HATE_SPEECH"].ToString(), out var hateSpeech))
                            {
                                message.HateSpeech = hateSpeech;
                            }

                            if (DateTime.TryParse(dictionary["date"].ToString(), out var date))
                            {
                                message.Date = date;
                            }

                            if (dictionary.TryGetValue("raw_message", out var rawMessage))
                            {
                                message.RawMessage = rawMessage.ToString();
                            }

                            if (dictionary.TryGetValue("filtered_message", out var filteredMessage))
                            {
                                message.FilteredMessage = filteredMessage.ToString();
                            }

                            if (dictionary.TryGetValue("account_id", out var accountId))
                            {
                                message.AccountId = accountId.ToString();
                            }

                            message.Id = Guid.NewGuid();
                            message.Date = dateProvider.CurrentSimulatedDate;
                        }
                        catch(Exception ex)
                        {
                            continue;
                        }

                        yield return message;
                    }
                }
            }  
        }
    }
}
