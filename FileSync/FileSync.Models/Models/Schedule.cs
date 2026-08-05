namespace FileSync.Models.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public bool RepeatDaily { get; set; }
        public bool RepeatWeekly { get; set; }
        public bool RepeatMonthly { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
