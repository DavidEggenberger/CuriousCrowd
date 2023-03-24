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

        public async IAsyncEnumerable<Message> ReadMessagesForAlliance(string allianceId, int skip = 0)
        {
            Random random = new Random();
            using (var reader = new StreamReader($@"Data\{DataConstants.MessageCSVFilePath}"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    await foreach (var expandoObject in csv.GetRecordsAsync<dynamic>().Skip(skip).Take(new Random().Next(14)))
                    {
                        var message = new Message();
                        try
                        {
                            var dictionary = (IDictionary<string, object>)expandoObject;

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

                            var randomized = random.Next(1, 100);

                            message.Reported = (message.Risk, randomized) switch
                            {
                                { Risk: > 6, randomized: > 10 } => true,
                                { Risk: > 5, randomized: > 30 } => true,
                                { Risk: > 4, randomized: > 30 } => true,
                                { Risk: > 3, randomized: > 50 } => true,
                                { Risk: > 2, randomized: > 70 } => true,
                                { Risk: > 1, randomized: > 80 } => true,
                                { Risk: > 0, randomized: > 90 } => true,
                                _ => false
                            };

                            message.Credibility = (new Random().Next(0, 100)) switch
                            {
                                > 90 => 90,
                                > 80 => 80,
                                > 70 => 70,
                                > 50 => 60,
                                > 30 => 30,
                                > 10 => 12
                            };

                            message.Id = Guid.NewGuid();
                            message.Date = dateProvider.CurrentSimulatedDate;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                        yield return message;
                    }
                }
            }
        }

        public async IAsyncEnumerable<Message> ReadDefaultRandomizedMessages(int skip = 0)
        {
            Random random = new Random();
            using (var reader = new StreamReader($@"Data\{DataConstants.MessageCSVFilePath}"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    await foreach (var expandoObject in csv.GetRecordsAsync<dynamic>().Skip(skip).Take(new Random().Next(13)))
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

                            var randomized = random.Next(1, 100);

                            message.Reported = (message.Risk, randomized) switch
                            {
                                { Risk: > 6, randomized: > 10 } => true,
                                { Risk: > 5, randomized: > 30 } => true,
                                { Risk: > 4, randomized: > 30 } => true,
                                { Risk: > 3, randomized: > 50 } => true,
                                { Risk: > 2, randomized: > 70 } => true,
                                { Risk: > 1, randomized: > 80 } => true,
                                { Risk: > 0, randomized: > 90 } => true,
                            };

                            message.Id = Guid.NewGuid();
                            message.Date = dateProvider.CurrentSimulatedDate;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                        yield return message;
                    }
                }
            }
        }

        public async IAsyncEnumerable<Message> ReadMessages(int skip = 0, int init = 1000)
        {
            Random random = new Random();
            using (var reader = new StreamReader($@"Data\{DataConstants.MessageCSVFilePath}"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    await foreach (var expandoObject in csv.GetRecordsAsync<dynamic>().Skip(skip).Take(init))
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

                            if (dictionary.TryGetValue("REPORTED_BY", out var reportedBy))
                            {
                                message.ReportedBy = reportedBy.ToString();
                            }

                            var randomized = random.Next(1, 100);

                            message.Reported = (message.Risk, randomized) switch
                            {
                                { Risk: > 6, randomized: > 10 } => true,
                                { Risk: > 5, randomized: > 30 } => true,
                                { Risk: > 4, randomized: > 30 } => true,
                                { Risk: > 3, randomized: > 50 } => true,
                                { Risk: > 2, randomized: > 70 } => true,
                                { Risk: > 1, randomized: > 80 } => true,
                                { Risk: > 0, randomized: > 90 } => true,
                            };

                            message.Credibility = (new Random().Next(0, 100)) switch
                            {
                                > 90 => 90,
                                > 80 => 80,
                                > 70 => 70,
                                > 50 => 50,
                                > 30 => 30,
                                > 10  => 10
                            };

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
