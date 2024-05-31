using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class MaterialsRequest
    {
        public int LearningObjId { get; set; }
        public string Name { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
    public static class MaterialsRequestExtension
    {
        public static MaterialsResponse ToMaterialsModel(this MaterialsRequest req)
        {
            var model = new MaterialsResponse()
            {
                LearningObjId = req.LearningObjId,
                Name = req.Name,
                CreateBy = req.CreateBy,
                CreateDate = DateTime.Now,
            };
            return model;
        }
    }
}

