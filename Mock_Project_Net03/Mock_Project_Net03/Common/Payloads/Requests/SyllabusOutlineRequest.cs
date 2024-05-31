using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;
using System.Net.WebSockets;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class SyllabusOutlineRequest
    {
        public int SyllabusId { get; set; }
        public string UnitName { get; set; }
        public string Description { get; set; }
        public int Time {  get; set; }

    }

    public static class SyllabusRequestExtenstion
    {
        public static TrainingProgramUnitModel ToUnitModel(this SyllabusOutlineRequest req)
        {
            var UnitModel = new TrainingProgramUnitModel()
            {
                SyllabusId = req.SyllabusId,
                UnitName = req.UnitName,
                Description = req.Description,
                Time = req.Time,

            };
            return UnitModel;
        }
    }
}
