using System;

namespace Client.BuildingBlocks
{
    public class DateProvider
    {
        public static DateTime CurrentSimulatedDate => new DateTime(2023, 03, 01, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
    }
}
