using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;

namespace Mock_Project_Net03.Dtos
{
    public class ClassTrainingUnitModel
    {
        public int Id { get; set; }

        public int TrainingProgramUnitId { get; set; }

        public int ClassId { get; set; }

        public int TrainerId { get; set; }

        public int RoomId { get; set; }

        public int Slot { get; set; }

        public DateTime Day { get; set; }

        public TrainingProgramUnitModel TrainingProgramUnit { get; set; }

        public ClassModel Class { get; set; }

        public UserModel Trainer { get; set; }

        public RoomModel Room { get; set; }
    }
}
