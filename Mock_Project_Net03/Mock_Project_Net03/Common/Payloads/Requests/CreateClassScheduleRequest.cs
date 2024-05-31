using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class CreateClassScheduleRequest
    {
        // public int ClassId { get; set; }
        // public int RoomId { get; set; }
        // public int Slot { get; set; }
        // public int TrainerId { get; set; }
        // public DateTime Date { get; set; }
        public int ClassId { get; set; }
        public int SemesterId { get; set; }
        public List<ScheduleDetail> ScheduleDetails { get; set; }
        
    }

    public class ScheduleDetail
    {
        public int SyllabusId { get; set; }
        public int Slot { get; set; }
        public int TrainerId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
    }
}
