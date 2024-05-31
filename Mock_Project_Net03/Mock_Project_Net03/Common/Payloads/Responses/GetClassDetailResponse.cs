using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class GetClassDetailResponse
    {
        public ClassModel? Class { get; set; }
        public List<Attendance> attendees { get; set; } // fix type to AttendanceModel if created
        public List<Class_TrainingUnitModel> ClassDetail { get; set; }
        public int AttendeeNumber {  get; set; }
        public int TrainerNumber {  get; set; }
        public string ClassTime {  get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string DeliveryType { get; set; }
    }
}
