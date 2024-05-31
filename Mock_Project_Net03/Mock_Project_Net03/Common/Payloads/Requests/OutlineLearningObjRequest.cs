using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class OutlineLearningObjRequest
    {
        public int UnitId { get; set; }
        public string LearningObjName { get; set; }
        public int OutputStandardId { get; set; }
        public DateTime TrainningTime { get; set; }
        public Boolean Method { get; set; }

    }
    public static class OutlineLearningObjExtension
    {
        public static LearningObjResponse ToLearningObjModel(this OutlineLearningObjRequest req)
        {
            var model = new LearningObjResponse()
            {
                UnitId = req.UnitId,
                Name = req.LearningObjName,
                OutputStandardId = req.OutputStandardId,
                TrainningTime = req.TrainningTime,
                Method = req.Method,
            };
            return model;
        }
    }
}
