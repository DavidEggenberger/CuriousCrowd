using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Server.Services
{
    public class AllianceLoaderService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public AllianceLoaderService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<List<Alliance>> ReadFirstAlliances()
        {
            var alliances = new List<Alliance>();
            using (var reader = new StreamReader($@"Data\{DataConstants.AllianceCSVFilePath}"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    await foreach (var expandoObject in csv.GetRecordsAsync<dynamic>())
                    {
                        var alliance = new Alliance();
                        try
                        {
                            var dictionary = (IDictionary<string, object>)expandoObject;

                            if (dictionary.TryGetValue("alliance_id", out object value))
                            {
                                alliance.AllianceId = value.ToString();
                            }

                            if (dictionary.TryGetValue("family_friendly", out object familyFriendliness))
                            {
                                alliance.FamilyFriendly = familyFriendliness.ToString() == "1" ? true : false;
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                        alliances.Add(alliance);

                        if(alliances.Count == 500)
                        {
                            break;
                        }
                    }
                }
            }

            return alliances;
        }
    }
}
