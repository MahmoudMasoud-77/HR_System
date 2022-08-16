namespace HRSystem.Models
{
    public class GeneralSettings
    {
        public int? Id { get; set; }
        public int ExtraTime { get; set; }
        public int Deduction { get; set; }

        public DayOfWeek Weekend1st { get; set; }
        public DayOfWeek? Weekend2nd { get; set; }
    }
}
