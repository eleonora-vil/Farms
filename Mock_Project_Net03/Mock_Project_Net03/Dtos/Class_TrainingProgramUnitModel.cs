using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Dtos
{
    public class Class_TrainingProgramUnitModel
    {
        public int Id { get; set; }
        public int TrainingProgramUnitId { get; set; }
        public int ClassId { get; set; }
        public int TrainerId { get; set; }
        public int RoomId { get; set; }
        public int Slot { get; set; }
        public DateTime Day { get; set; }
        public TrainingProgramUnit TrainingProgramUnit { get; set; }
        public ClassIntructorModel Class { get; set; }
        public InstructorModel Trainer { get; set; }
        public Room Room { get; set; }
    }
}
