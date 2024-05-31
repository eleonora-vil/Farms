﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;

namespace Mock_Project_Net03.Dtos
{
    [Table("Class_TrainingUnit")]
    public class Class_TrainingUnitModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TrainingProgramUnitId { get; set; }
        public int ClassId { get; set; }
        public int TrainerId { get; set; }
        public int RoomId { get; set; }
        public int Slot { get; set; }
        public DateTime Day { get; set; }

        [ForeignKey("TrainingProgramUnitId")]
        public TrainingProgramUnitModel TrainingProgramUnit { get; set; }
    }
}