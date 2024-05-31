using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class GetDetailsSyllabus
    {
        public GetSyllabusByIdResponse getSyllabusResponse { get; set; }
        public List<OutputStandardModel> outputStandardModels { get; set; }
    }
}
