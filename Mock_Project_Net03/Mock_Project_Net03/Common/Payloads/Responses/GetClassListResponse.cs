using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class GetClassListResponse
    {
        public IEnumerable<ClassModel>? Classes { get; set; }
        public int TotalPages { get; set; }
        public string? Message { get; set; }
    }
}
