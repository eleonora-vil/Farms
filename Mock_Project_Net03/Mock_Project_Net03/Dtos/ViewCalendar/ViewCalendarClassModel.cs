using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Dtos.ViewCalendar
{
    public class ViewCalendarClassModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int ProgramId { get; set; }
        public int InstructorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ClassTime Time { get; set; }
        public string Status { get; set; }
    }
}
