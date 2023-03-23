using CsvHelper.Configuration.Attributes;
using System;

namespace Server.Data
{
    public class Message
    {
        public string AccountId { get; set; }
        public Guid Id { get; set; }

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

        public string RawMessage { get; set; }
        public string FilteredMessage { get; set; }


        public double? HarmfullnessScore => CalculateHarfulness();

        public Alliance Alliance { get; set; }

        public bool Reported { get; set; }

        private double CalculateHarfulness()
        {
            double result = 1;
            if (HateSpeech.HasValue)
            {
                result *= Math.Max(1, HateSpeech.Value);
            }
            if (Violence.HasValue)
            {
                result *= Math.Max(1, Violence.Value);
            }
            if (Fraud.HasValue)
            {
                result *= Math.Max(1, Fraud.Value);
            }
            if (Vulgarity.HasValue)
            {
                result *= Math.Max(1, Vulgarity.Value);
            }
            if (Bullying.HasValue)
            {
                result *= Math.Max(1, Bullying.Value);
            }
            return result;
        }
    }
}
