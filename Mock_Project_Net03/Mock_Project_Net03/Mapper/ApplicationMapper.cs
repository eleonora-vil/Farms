using AutoMapper;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;
using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() 
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<User, CreateUserModel>().ReverseMap();
            CreateMap<UserRole, UserRoleModel>().ReverseMap();
            CreateMap<Permission, PermissionModel>().ReverseMap();
            CreateMap<Syllabus, SyllabusModel>().ReverseMap();
            CreateMap<Syllabus, Syllabus_CreateSyllabusModel>().ReverseMap();
            CreateMap<Syllabus, GetSyllabusByIdResponse>().ReverseMap();
            CreateMap<Class_TrainingUnit, Class_TrainingUnitModel>().ReverseMap();
            CreateMap<LearningObj, LearningObjModel>().ReverseMap();
//            CreateMap<Materials, MaterialsModel>().ReverseMap();
            CreateMap<Class, ClassModel>().ReverseMap();
            CreateMap<Materials, MaterialsResponse>().ReverseMap();
            CreateMap<AssessmentScheme, AssessmentSchemeModel>().ReverseMap();
            CreateMap<TrainingProgram, TrainingProgramModel>().ReverseMap();
            CreateMap<TrainingProgramUnit, TrainingProgramUnitModel>().ReverseMap();
            CreateMap<LearningObj,LearningObjResponse>().ReverseMap();
            CreateMap<Materials, MaterialModel>().ReverseMap();
            CreateMap<AssessmentScheme_Syllabus, AssessmentScheme_CreateSyllabusModel>().ReverseMap();
            CreateMap<TrainingProgramUnit, TrainingProgramUnit_CreateSyllabusModel>().ReverseMap();
            CreateMap<LearningObj, LearningObj_CreateSyllabusModel>().ReverseMap();
            CreateMap<OutputStandard, OutputStandardModel>().ReverseMap();
            CreateMap<AssessmentScheme, AssessmentScheme_ToAdd>().ReverseMap();
            CreateMap<Syllabus, UpdateSyllabusModel>().ReverseMap();
            CreateMap<AssessmentScheme_Syllabus, AssessmentSchemeUpdateSyllabusModel>().ReverseMap();
            CreateMap<CreateSyllabusModel, Syllabus>().ReverseMap();
            CreateMap<UpdateSyllabusModel, CreateSyllabusModel>().ReverseMap();
        }
    }
}
