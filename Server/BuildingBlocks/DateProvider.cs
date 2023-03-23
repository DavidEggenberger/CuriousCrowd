using System;

namespace Server.BuildingBlocks
{
    public class DateProvider
    {
        public DateTime CurrentSimulatedDate => new DateTime(2023, 03, 01, DateTime.Now.Hour, DateTime.Now.Minute, new Random().Next(DateTime.Now.Second, DateTime.Now.Second + 10));
    }
}
