using CsvHelper.Configuration.Attributes;
using System;

namespace Server.Data
{
    public class Message
    {
        [Name("is_family_friendly")]
        public double? FamilyFriendly { get; set; }
        
        [Name("risk")]
        public double? Risk { get; set; }
        
        [Name("VIOLENCE")]
        public double? Violence { get; set; }
        
        [Name("BULLYING")]
        public double? Bullying { get; set; }
        
        [Name("VULGARITY")]
        public double? Vulgarity { get; set; }
        
        [Name("ALARM")]
        public double? Alarm { get; set; }
        
        [Name("FRAUD")]
        public double? Fraud { get; set; }
        
        [Name("HATE_SPEECH")]
        public double? HateSpeech { get; set; }
        
        [Name("date")]
        public DateTime Date { get; set; }

        [Name("timestamp")]
        public DateTimeOffset TimeStamp { get; set; }
    }
}
