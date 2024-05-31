using Mock_Project_Net03.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse
{
    public class LearningObjResponse
    {
        public int LearningObjId { get; set; }

        public string Name { get; set; }

        public DateTime TrainningTime { get; set; }

        public bool Method { get; set; }

        public int Index { get; set; }

        public string? Status { get; set; }
        public string? DeliveryType { get; set; }

        public int? OutputStandardId { get; set; }
        public int UnitId { get; set; }
        public string? Duration { get; set; }

        public TrainingProgramUnitModel Unit { get; set; }

        public OutputStandardModel OutputStandard { get; set; }

        public List<MaterialsResponse> Material { get; set; }

    }
}
