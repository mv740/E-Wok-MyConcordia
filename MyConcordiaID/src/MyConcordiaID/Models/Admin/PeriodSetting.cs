using System;

namespace MyConcordiaID.Models.Admin
{
    /// <summary>
    ///  Model Class for valid picture update period 
    /// </summary>
    public class PeriodSetting
    {
        public int Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
