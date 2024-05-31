using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;

namespace Mock_Project_Net03.Services
{
    public class EnrollmentService
    {
        private readonly IRepository<Enrollment, int> _enrollmentRepository;
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository;
        private readonly IRepository<Class, int> _classRepository;
        private readonly IMapper _mapper;
        public EnrollmentService(IRepository<Enrollment, int> enrollmentRepository, IRepository<User, int> userRepository, IRepository<Class, int> classRepository, IRepository<Class_TrainingUnit, int> classTrainingUnitRepository, IMapper mapper)
        {
            _enrollmentRepository = enrollmentRepository;
            _classRepository = classRepository;
            _classTrainingUnitRepository = classTrainingUnitRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public List<UserModel> GetAllUserEnrollInClass(int classId)
        {
            var enrollments = _enrollmentRepository.FindByCondition(x => x.ClassId == classId);
            var users = enrollments.Select(x => x.Trainee).ToList();
            var usersModel = _mapper.Map<List<UserModel>>(users);
            return usersModel;
        }
        
        public async Task<bool> RemoveEnrolledStudent(int id)
        {
            var search =_enrollmentRepository.FindByCondition(x => x.EnrollmentId ==  id).FirstOrDefault();
            if(search == null)
            {
                throw new NotFoundException("EnrollmenttId doesn't exist!");
            }
            var remove = _enrollmentRepository.Remove(id);
            var save = await _enrollmentRepository.Commit();
            if (save> 0)
            {
                return true;
            }
            return false;
        }

        public async Task<EnrollmentModel> AddStudentToClass(EnrollmentRequest enrollmentModel)
        {
            try
            {
                var checkRole = await _userRepository.FindByCondition(x => x.UserId == enrollmentModel.TraineeId).Select(x => x.RoleID).FirstOrDefaultAsync();
                if (checkRole != 4)
                {
                    throw new Exception("You have no permission!");
                }
                var checkExistClass = _classRepository.FindByCondition(x => x.ClassId == enrollmentModel.ClassId).FirstOrDefault();
                if (checkExistClass == null)
                {
                    throw new Exception("Class is not exist!");
                }
                var findStudent = await _enrollmentRepository.FindByCondition(x => x.TraineeId == enrollmentModel.TraineeId && x.ClassId == enrollmentModel.ClassId).FirstOrDefaultAsync();
                var nameClass = await _classRepository.FindByCondition(x => x.ClassId == enrollmentModel.ClassId).Select(x => x.ClassName).FirstOrDefaultAsync();
                if (findStudent != null)
                {
                    throw new Exception("This student is already in this " + nameClass + " classroom");
                }
                var checkExistClassTrainingUnit = await _classTrainingUnitRepository.FindByCondition(x => x.ClassId == enrollmentModel.ClassId).FirstOrDefaultAsync();
                if (checkExistClassTrainingUnit != null)
                {
                    var getClassByTrainee = await _enrollmentRepository.GetAll().Include(x => x.Class).Where(x => x.TraineeId == enrollmentModel.TraineeId).Select(x => x.ClassId).ToListAsync();
                    var days = await _classTrainingUnitRepository
                   .GetAll()
                   .Where(x => getClassByTrainee.Contains(x.ClassId))
                   .Include(x => x.Class)
                   .ToListAsync();
                    var checkEnterClassID = await _classTrainingUnitRepository.FindByCondition(x => x.ClassId == enrollmentModel.ClassId).ToListAsync();
                    foreach (var c in checkEnterClassID)
                    {
                        foreach (var day in days)
                        {
                            if (c.Day.Date == day.Day.Date && c.Slot == day.Slot)
                            {
                                throw new Exception("The slot have existed!");
                            }
                        }
                    }
                }
                var enrollmentEntity = _mapper.Map<Enrollment>(enrollmentModel);
                await _enrollmentRepository.AddAsync(enrollmentEntity);
                await _enrollmentRepository.Commit();
                return _mapper.Map<EnrollmentModel>(enrollmentEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
