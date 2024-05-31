using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Repositories;
using System.Drawing.Printing;
using System.Linq;

namespace Mock_Project_Net03.Services
{
    public class ManaService
    {
        private readonly IRepository<User, int> _userRepository;

        private readonly IRepository<Entities.Syllabus, int> _syllabusRepository;

        private readonly IRepository<TrainingProgram, int> _trainingProgramRepository;

        private readonly IRepository<Class, int> _classRepository;

        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepository;

        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository;

        private readonly IRepository<LearningObj, int> _learningObjRepository;

        private readonly IMapper _mapper;

        public ManaService(IRepository<User, int> userRepository,
                       IRepository<Entities.Syllabus, int> syllabusRepository,
                                  IRepository<TrainingProgram, int> trainingProgramRepository,
                                             IRepository<Class, int> classRepository,
                                                        IRepository<TrainingProgramUnit, int> trainingProgramUnitRepository,
                                                                   IRepository<Class_TrainingUnit, int> classTrainingUnitRepository,
                                                                              IRepository<LearningObj, int> learningObjRepository,
                                                                                         IMapper mapper)
        {
            _userRepository = userRepository;
            _syllabusRepository = syllabusRepository;
            _trainingProgramRepository = trainingProgramRepository;
            _classRepository = classRepository;
            _trainingProgramUnitRepository = trainingProgramUnitRepository;
            _classTrainingUnitRepository = classTrainingUnitRepository;
            _learningObjRepository = learningObjRepository;
            _mapper = mapper;
        }




        

        public async Task<ManaModel> GetTotalAllToMana()
        {
            var totalUser = await _userRepository.GetAll().Where(x => x.Status.Equals("Active")).CountAsync();
            var totalSyllabus = await _syllabusRepository.CountAsync();
            var totalTrainingProgram = await _trainingProgramRepository.CountAsync();
            var totalClass = await _classRepository.CountAsync();
            var totalRoleSuperAdmin = await _userRepository.CountAsync(x => x.RoleID == 1);
            var totalRoleAdmin = await _userRepository.CountAsync(x => x.RoleID == 2);
            var totalRoleTrainer = await _userRepository.CountAsync(x => x.RoleID == 3);
            var totalRoleTrainee = await _userRepository.CountAsync(x => x.RoleID == 4);
            var syllabusQuery = await _syllabusRepository
                                .GetAll()
                                .Include(x => x.Instructor)
                                .Include(x => x.TrainingProgram_Syllabus)
                                .ThenInclude(x => x.TrainingProgram)
                                .OrderByDescending(x => x.CreatedDate).ToArrayAsync();

            var syllabusList = syllabusQuery
                                .Take(3);

            var listSyllabusModel = new List<SyllabusModel>();

            foreach (var sy in syllabusList)
            {
                var trainningProgramIds = sy.TrainingProgram_Syllabus
                                          .Select(x => x.TrainingProgramId).ToList();
                var classTrainingUnits = _classTrainingUnitRepository.GetAll()
                                         .Where(x => trainningProgramIds.Contains((int)x.Class.ProgramId))
                                         .Include(x => x.Class)
                                         .ThenInclude(x => x.Program)
                                         .ToList();


                var listTrainingProgramUnitIds = classTrainingUnits
                                                 .Select(x => x.TrainingProgramUnitId).ToList();
                var listUnitIds = _trainingProgramUnitRepository.GetAll()
                                                 .Where(x => listTrainingProgramUnitIds.Contains(x.UnitId))
                                                 .Select(x => x.UnitId).ToList();
                var listLearningObjectIds = _learningObjRepository.GetAll()
                                                                     .Where(x => listUnitIds.Contains((int)x.UnitId))
                                                 .Include(x => x.OutputStandard).ToList();
                var outPutStandards = _mapper.Map<List<OutputStandardModel>>(listLearningObjectIds
                                                 .Select(x => x.OutputStandard)
                                                 .DistinctBy(x => x.OutputStandardId));

                listSyllabusModel.Add(new SyllabusModel
                {
                    SyllabusId = sy.SyllabusId,
                    Name = sy.Name,
                    Code = sy.Code,
                    CreatedDate = sy.CreatedDate,
                    InStructorName = sy.Instructor?.FullName,
                    Slot = classTrainingUnits.Count,
                    OutputStandards = outPutStandards.ToList(),
                    Status = sy.Status
                });
            }
            ManaModel manaModel = new ManaModel
            {
                totalSyllabus = totalSyllabus,
                totalTrainingProgram = totalTrainingProgram,
                totalClass = totalClass,
                totalUser = totalUser,
                totalRoleSuperAdmin = totalRoleSuperAdmin,
                totalRoleAdmin = totalRoleAdmin,
                totalRoleTrainer = totalRoleTrainer,
                totalRoleTrainee = totalRoleTrainee,
                listSyllabus = listSyllabusModel.ToList()
            };
            return manaModel;
        }
    }
}
