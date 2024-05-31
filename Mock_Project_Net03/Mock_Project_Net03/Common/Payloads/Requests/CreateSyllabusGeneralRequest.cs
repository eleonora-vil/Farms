using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class CreateSyllabusGeneralRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Level { get; set; }
        public string Version { get; set; }
        public string TechnicalRequirement { get; set; }
        public string CourseObjectives { get; set; }
        public int AttendeeNumber { get; set; }
    }
    public static class CreateSyllabusGeneralExtenstion
    {
        public static SyllabusModel ToSyllabusModel (this CreateSyllabusGeneralRequest createSyllabusGeneral)
        {
            var model = new SyllabusModel()
            {
                Name = createSyllabusGeneral.Name,
                Code = createSyllabusGeneral.Code,
                Description = "Enter Description",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Outline = "",
                Level = createSyllabusGeneral.Level,
                Version = createSyllabusGeneral.Version,
                TechnicalRequirement = createSyllabusGeneral.TechnicalRequirement,
                CourseObjectives = createSyllabusGeneral.CourseObjectives,
                TrainingDelivery = "",
                AttendeeNumber = createSyllabusGeneral.AttendeeNumber,
                Status = "Active",
                InstructorId = 1,
            };
            return model;
        }
    }
    
}
