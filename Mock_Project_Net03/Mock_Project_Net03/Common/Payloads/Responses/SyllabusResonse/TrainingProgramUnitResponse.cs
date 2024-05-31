using Mock_Project_Net03.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse
{
    public class TrainingProgramUnitResponse
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }
        public string Status { get; set; }

        public int? Index { get; set; }

        public int SyllabusId { get; set; }

        [ForeignKey("SyllabusId")]
        public Syllabus Syllabus { get; set; }
        public List<LearningObjResponse> LearningObjs { get; set; } = new List<LearningObjResponse>();
    }
}
