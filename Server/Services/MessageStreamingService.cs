using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
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
        public MessageStreamingService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async IAsyncEnumerable<Message> ReadMessages()
        {
            using (var reader = new StreamReader($@"{webHostEnvironment.ContentRootPath}\Data\{DataConstants.MessageCSVFilePath}"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    await foreach (var expandoObject in csv.GetRecordsAsync<dynamic>())
                    {
                        var message = new Message();
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

                        if (DateTimeOffset.TryParse(dictionary["timestamp"].ToString(), out var timestamp))
                        {
                            message.TimeStamp = timestamp;
                        }

                        if (dictionary.TryGetValue("raw_message", out var rawMessage))
                        {
                            message.RawMessage = rawMessage.ToString();
                        }

                        if (dictionary.TryGetValue("filtered_message", out var filteredMessage))
                        {
                            message.FilteredMessage = filteredMessage.ToString();
                        }

                        yield return message;
                    }
                }
            }  
        }
    }
}
