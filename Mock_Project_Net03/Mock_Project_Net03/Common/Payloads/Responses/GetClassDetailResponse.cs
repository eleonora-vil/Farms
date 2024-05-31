using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class GetClassDetailResponse
    {
        public ClassModel? Class { get; set; }
        public List<Attendance> attendees { get; set; }
    }
}
