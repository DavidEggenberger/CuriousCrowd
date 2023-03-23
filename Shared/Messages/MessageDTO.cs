using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public class MessageDTO
    {
        public bool Reported { get; set; }

        public string AccountId { get; set; }
        public Guid Id { get; set; }
        public string ReportedBy { get; set; }
        public double? FamilyFriendly { get; set; }
        public double? Risk { get; set; }
        public double? Violence { get; set; }
        public double? Bullying { get; set; }
        public double? Vulgarity { get; set; }
        public double? Alarm { get; set; }
        public double? Fraud { get; set; }
        public double? HateSpeech { get; set; }
        public DateTime Date { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string RawMessage { get; set; }
        public string FilteredMessage { get; set; }
        public AllianceDTO Alliance { get; set; }
        public int Credibility { get; set; }
        public double? HarmfullnessScore { get; set; }
    }
}
