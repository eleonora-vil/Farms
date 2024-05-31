using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Common.Payloads.Requests;
using Microsoft.IdentityModel.Tokens;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using NuGet.Protocol;
using static Mock_Project_Net03.Dtos.ViewCalendar.ViewTrainingCalendarModel;

namespace Mock_Project_Net03.Services
{
    public class ClassService
    {
        private readonly IRepository<Class, int> _classRepository;
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository;
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<Enrollment, int> _enrollmentRepository;
        private readonly IRepository<Attendance, int> _attendanceRepository;
        private readonly IRepository<UserRole, int> _roleRepository;
        private readonly IRepository<TrainingProgram, int> _trainingProgramRepository;
        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepository;
        private readonly IRepository<Semester, int> _semesterRepository;
        private readonly IRepository<Entities.Syllabus, int> _syllabusRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<TrainingProgram_Syllabus, int> _program_syllabus;

        public ClassService(IRepository<Class, int> classRepository, IRepository<Attendance, int> attendanceRepository,
            IRepository<Class_TrainingUnit, int> classTrainingUnitRepository, IRepository<User, int> userRepository,
            IRepository<Enrollment, int> enrollmentRepository, IRepository<TrainingProgram, int> trainingProgramRepository,
            IRepository<TrainingProgramUnit, int> trainingProgramRepositoryUnit,
            IRepository<UserRole, int> roleRepository, IRepository<Semester, int> semesterRepository,
            IRepository<Entities.Syllabus, int> syllabusRepository, 
            IRepository<TrainingProgram_Syllabus, int> program_syllabus,
            IMapper mapper)
        {
            _classRepository = classRepository;
            _classTrainingUnitRepository = classTrainingUnitRepository;
            _userRepository = userRepository;
            _enrollmentRepository = enrollmentRepository;
            _attendanceRepository = attendanceRepository;
            _trainingProgramRepository = trainingProgramRepository;
            _trainingProgramUnitRepository = trainingProgramRepositoryUnit;
            _roleRepository = roleRepository;
            _semesterRepository = semesterRepository;
            _syllabusRepository = syllabusRepository;
            _mapper = mapper;
            _program_syllabus = program_syllabus;
        }

        public async Task<ClassModel> GetClassById(int id)
        {
            var classEntity = await _classRepository
                .GetByIdAsync(id);
            if (classEntity == null)
            {
                throw new NotFoundException("Class not found");
            }

            classEntity.Semester = await _semesterRepository.GetByIdAsync((int)classEntity.SemesterId);
            classEntity.Instructor = await _userRepository.GetByIdAsync((int)classEntity.InstructorId);
            classEntity.Program = await _trainingProgramRepository.GetByIdAsync((int)classEntity.ProgramId);

            return _mapper.Map<ClassModel>(classEntity);
        }

        public async Task<(List<ClassModel>, int totalPages)> GetAllClass(int pageNumber, int pageSize)
        {
            var totalItems = await _classRepository.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var classList = _classRepository
                .GetAll()
                .Include(x => x.Instructor)
                .Include(x => x.Program)
                .Include(x => x.Semester)
                .OrderBy(x => x.ClassId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (classList.Count == 0)
            {
                throw new NotFoundException("Currently no records");
            }

            var listClassModel = new List<ClassModel>();
            foreach (var item in classList)
            {
                listClassModel.Add(_mapper.Map<ClassModel>(item));
            }

            return (listClassModel, totalPages);
        }

        public async Task<List<ClassModel>> GetAllClassWithFilter(
            List<ClassModel> classModels,
            string? KeyWord,
            string? Status,
            string? Semester,
            int TrainerId
        )
        {
            var listClassModelWithFilter = classModels;

            if (!string.IsNullOrEmpty(KeyWord))
            {
                listClassModelWithFilter = listClassModelWithFilter
                    .Where(x => x.ClassName.ToLower().Contains(KeyWord.ToLower())
//                    || x.ClassCode.ToLower().Contains(KeyWord.ToLower())   dòng này đang bị lỗi do trong db ClassCode là null
                    ).ToList(); // class name match keyword
            }

            if (!string.IsNullOrEmpty(Semester))
            {
                listClassModelWithFilter = listClassModelWithFilter
                    .Where(x => x.Semester.SemesterName.ToLower().Contains(Semester.ToLower())).ToList();
            }

            if (TrainerId != 0)
            {
                listClassModelWithFilter = listClassModelWithFilter
                    .Where(x => x.InstructorId == TrainerId).ToList(); // compare trainer to instructor's username ?
            }

            if (!string.IsNullOrEmpty(Status))
            {
                listClassModelWithFilter = listClassModelWithFilter
                    .Where(x => Status.ToLower().Equals(x.Status.ToLower())).ToList();
                if (Status.ToLower().Contains("deactive"))
                {
                    listClassModelWithFilter = listClassModelWithFilter
                        .Where(x => !x.Status.ToLower().Equals("active")).ToList();
                }
            }

            if (!listClassModelWithFilter.Any())
            {
                throw new NotFoundException("There's no record matching with your filter");
            }

            return listClassModelWithFilter.ToList();
        }

        public async Task<GetClassDetailResponse> GetClassDetail(int id)
        {
            GetClassDetailResponse response = new GetClassDetailResponse();

            var classEntity = await GetClassById(id);

            if (classEntity == null)
            {
                throw new NotFoundException("Class not found");
            }

            var attendees = _attendanceRepository
                .GetAll()
                .Where(x => x.ClassId == classEntity.ClassId)
                .ToList();

            foreach(var item in attendees)
            {
                item.Class = null;
            }

            int attendeesNumber = attendees.Count();

//          get thêm data trong semeter, trainer + count, room, slot 

            var classDetail = _classTrainingUnitRepository
                .GetAll()
                .Include(x => x.TrainingProgramUnit)
                .Where(x => x.ClassId == classEntity.ClassId)
                .ToList();

            int trainersNumber = 0; // initialize trainer number 
            var countTrainer = new List<int>(); // a list contain trainerId are already counted
            var classDetailModel = new List<Class_TrainingUnitModel>();
            var syllabus = new Entities.Syllabus();
            response.DeliveryType = "";
            foreach (var item in classDetail)
            {
                classDetailModel.Add(_mapper.Map<Class_TrainingUnitModel>(item));

                // counting start
                if (countTrainer.Contains(item.TrainerId)) // if trainer are already counted
                {
                    continue; // skip
                }

                // else
                countTrainer.Add(item.TrainerId); // checked that trainer are counted
                trainersNumber++; // number of trainer nymber +1
                syllabus = await _syllabusRepository.GetByIdAsync((int)item.TrainingProgramUnit.SyllabusId);

                if (response.DeliveryType == "") 
                {
                    response.DeliveryType = syllabus.TrainingDelivery;
                }
                if (!response.DeliveryType.ToLower().Contains(syllabus.TrainingDelivery.ToLower()))
                {
                    response.DeliveryType += $", {syllabus.TrainingDelivery}";
                }
                
            }

            response.Class = _mapper.Map<ClassModel>(classEntity);
            response.attendees = attendees;
            response.ClassDetail = classDetailModel;
            response.AttendeeNumber = attendeesNumber;
            response.TrainerNumber = trainersNumber;

            response.CreatedDate = classEntity.Program.CreateDate == null
                ? DateTime.MinValue
                : (DateTime)classEntity.Program.CreateDate;
            response.ModifiedDate = classEntity.Program.LastModifiedDate == null
                ? DateTime.MinValue
                : (DateTime)classEntity.Program.LastModifiedDate;
            response.CreatedBy = classEntity.Program.CreateBy;
            response.ModifiedBy = classEntity.Program.LastUpdatedBy;

            if (!classDetail.IsNullOrEmpty())
            {
                response.ClassTime = "Not secheduled";
                switch (classDetail.FirstOrDefault().Slot)
                {
                    case 1:
                        response.ClassTime = "8:00 - 12:00";
                        break;
                    case 2:
                        response.ClassTime = "13:00 - 17:00";
                        break;
                    case 3:
                        response.ClassTime = "18:00 - 22:00";
                        break;
                    default:
                        response.ClassTime = "Not secheduled";
                        break;
                }
            }

            return response;
        }

        public async Task<List<UserModel>> SearchFreeInstructors(int slot, DateTime day)
        {
            try
            {
                var ins = await _classTrainingUnitRepository
                    .FindByCondition(i => i.Slot == slot && i.Day.Date == day.Date)
                    .Select(i => i.TrainerId).ToListAsync();

                var instructorList = _userRepository.GetAll()
                    .Where(u => !ins.Contains(u.UserId) && u.UserRole.RoleName == "Instructor").ToList();

                var instructorListModel = _mapper.Map<List<UserModel>>(instructorList);
                return instructorListModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ClassModel> UpdateStatusClass(int id, string status)
        {
            var processStatus = char.ToUpper(status[0]) + status.Substring(1).ToLower();
            var searchClass = _classRepository.FindByCondition(o => o.ClassId == id).FirstOrDefault();
            if (searchClass == null)
            {
                throw new BadRequestException("ClassID does not exist!");
            }

            searchClass.Status = processStatus;
            _classRepository.Update(searchClass);
            var result = await _classRepository.Commit();
            if (result > 0)
            {
                var viewClass = await GetClassById(id);
                return viewClass;
            }

            return null;
        }

        public async Task<ClassModel> AddTrainingProgramToClass(int classId, int trainingProgramId, int instructorId,
            int roomId, int slot)
        {
            var classEntity = await _classRepository.GetByIdAsync(classId);
            var trainingProgram = await _trainingProgramRepository.GetByIdAsync(trainingProgramId);
            var instructor = await _userRepository.GetByIdAsync(instructorId);
            var trainingProgramUnit = _trainingProgramUnitRepository.GetAll()
                .Where(i => i.Index == trainingProgramId)
                .ToList();

            if (classEntity == null)
            {
                throw new NotFoundException("Class not found!");
            }

            if (trainingProgram == null)
            {
                throw new NotFoundException("Training program not found!");
            }

            if (instructor == null)
            {
                throw new NotFoundException("Instructor not found!");
            }

            var classTrainingUnit = new Class_TrainingUnitModel();
            try
            {
                foreach (var item in trainingProgramUnit)
                {
                    classTrainingUnit = new Class_TrainingUnitModel()
                    {
                        TrainingProgramUnitId = item.UnitId,
                        ClassId = classId,
                        TrainerId = instructorId,
                        RoomId = roomId, // get the inputted room to be the default room of all study day
                        Slot = slot, // get the inputted slot (study time) to be the default slot of all study day
                    };

                    if (classTrainingUnit != null)
                    {
                        await _classTrainingUnitRepository.AddAsync(_mapper.Map<Class_TrainingUnit>(classTrainingUnit));
                        await _classTrainingUnitRepository.Commit();
                    }
                }

                classEntity.ProgramId = trainingProgramId;
                classEntity.InstructorId = instructorId;
                _classRepository.Update(classEntity);
                await _classRepository.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _mapper.Map<ClassModel>(classEntity);
        }

        public async Task<ClassModel> CreateNewClass(ClassModel classModel)
        {
            var existedClass = _classRepository.FindByCondition(x => x.ClassName.Equals(classModel.ClassName))
                .FirstOrDefault();
            if (existedClass is not null)
            {
                throw new BadRequestException("Class name already exist");
            }

            var existedSemester = _classRepository.FindByCondition(x => x.SemesterId == classModel.SemesterId)
                .FirstOrDefault();
            if (existedSemester is null)
            {
                throw new BadRequestException("Semester does not exist");
            }

            if (classModel.InstructorId != null)
            {
                var exitstedInstructor = _userRepository.FindByCondition(x => x.UserId == classModel.InstructorId)
                    .FirstOrDefault();
                if (exitstedInstructor is null)
                {
                    throw new BadRequestException("Admin does not exist");
                }

                var existedRole = _roleRepository.FindByCondition(x => x.RoleId == exitstedInstructor.RoleID)
                    .FirstOrDefault();
                if (existedRole is null)
                {
                    throw new BadRequestException("User role does not exist");
                }

                if (!existedRole.RoleName.Equals("Admin"))
                {
                    throw new BadRequestException("User role is not admin");
                }
            }

            if (classModel.ProgramId != null)
            {
                var existedProgram = _trainingProgramRepository
                    .FindByCondition(x => x.ProgramId == classModel.ProgramId).FirstOrDefault();
                if (existedProgram is null)
                {
                    throw new BadRequestException("Program does not exist");
                }
            }

            var classEntity = _mapper.Map<Class>(classModel);
            classEntity.Status = "InActive";
            classEntity = await _classRepository.AddAsync(classEntity);
            int result = await _userRepository.Commit();
            if (result > 0)
            {
                classModel.ClassId = classEntity.ClassId;
                classModel.Status = classEntity.Status;
                return classModel;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Class_TrainingProgramUnitModel>> GetClassSchedules(int classId)
        {
            var classEntities = await _classTrainingUnitRepository.FindByCondition(x => x.ClassId == classId)
                .Include(x => x.Trainer)
                .Include(x => x.Class).ThenInclude(x => x.Semester)
                .Include(x => x.TrainingProgramUnit)
                .Include(x => x.Room)
                .ToListAsync();

            var listModel = _mapper.Map<List<Class_TrainingProgramUnitModel>>(classEntities);

            foreach (var classEntity in classEntities)
            {
                foreach (var model in listModel)
                {
                    var Class = _mapper.Map<ClassIntructorModel>(classEntity.Class);
                    var Trainer = _mapper.Map<InstructorModel>(classEntity.Trainer);
                    model.Trainer = Trainer;
                    model.Class = Class;
                }
            }

            if (classEntities != null && classEntities.Any())
            {
                return listModel;
            }

            return new List<Class_TrainingProgramUnitModel>();
        }

        public async Task<bool> CheckProgramAvailableToUpdate(int id)
        {
            var check = await _classRepository.FindByCondition(x => x.ProgramId == id).FirstOrDefaultAsync();
            if (check is not null)
            {
                return false;
            }

            return true;
        }

        public bool IsStarted(int classId)
        {
            var existedClass = _classRepository.FindByCondition(x => x.ClassId == classId).FirstOrDefault();
            if (existedClass is null)
            {
                throw new BadRequestException("This class does not exist");
            }

            var semester = _semesterRepository.FindByCondition(x => x.SemesterID == existedClass.SemesterId)
                .FirstOrDefault();
            if (semester is null)
            {
                throw new BadRequestException("There is no semester");
            }

            if (DateTime.Now.Date > semester.SemesterStartDate)
            {
                return false;
            }

            return true;
        }

        public async Task<ClassModel?> UpdateClass(UpdateClassRequest updateClass, int classId)
        {
            if (!string.IsNullOrEmpty(updateClass.ClassName) && !string.IsNullOrEmpty(updateClass.ClassCode)
                                                             && updateClass.ProgramId is null
                                                             && updateClass.InstructorId is null
                                                             && updateClass.SemesterId is null)
            {
                return null;
            }

            var existedClass = _classRepository.FindByCondition(c => c.ClassId == classId).First();
            existedClass.ClassName = string.IsNullOrEmpty(updateClass.ClassName)
                ? existedClass.ClassName
                : updateClass.ClassName;
            existedClass.ClassCode = string.IsNullOrEmpty(updateClass.ClassCode)
                ? existedClass.ClassCode
                : updateClass.ClassCode;

            if (updateClass.ProgramId is not null)
            {
                var program = _trainingProgramRepository.FindByCondition(tp => tp.ProgramId == updateClass.ProgramId)
                    .FirstOrDefault();
                existedClass.Program = program;
            }

            if (updateClass.InstructorId is not null)
            {
                var instructor = _userRepository.FindByCondition(i => i.UserId == updateClass.InstructorId)
                    .FirstOrDefault();
                existedClass.Instructor = instructor;
            }

            if (updateClass.SemesterId is not null)
            {
                var semester = _semesterRepository.FindByCondition(s => s.SemesterID == updateClass.SemesterId)
                    .FirstOrDefault();
                existedClass.Semester = semester;
            }

            var classEntity = _mapper.Map<Class>(existedClass);
            var result = _classRepository.Update(classEntity);
            if (await _classRepository.Commit() > 0 ? true : false)
            {
                return _mapper.Map<ClassModel>(result);
            }

            throw new BadRequestException("Something went wrong");
        }

        public async Task<AddAttendeeResponse> AddAttendeeToClass(int classId, int attendeeId)
        {
            string message = null;
            var classesOfAttendee = _attendanceRepository
                .GetAll()
                .Where(x => x.TraineeId == attendeeId && x.Status == "Active")
                .Include(x => x.Trainee)
                .Include(x => x.Class)
                .ToList();

            var classInfo = _classTrainingUnitRepository
                .GetAll()
                .Where(x => x.ClassId == classId && x.Day.CompareTo(DateTime.Now) > 0)
                .ToList();

            if (attendeeId < 0)
            {
                throw new Exception("Invalid attendee id");
            }

            if (await _classRepository.GetByIdAsync(classId) == null)
            {
                throw new NotFoundException("Class does not existed");
            }

            var checkBusyClass = new List<Class_TrainingUnit>();

            foreach (var item in classesOfAttendee)
            {
                if (item.ClassId == classId)
                {
                    throw new Exception("Attendee already in class");
                }

                checkBusyClass = _classTrainingUnitRepository
                    .GetAll()
                    .Where(x => x.ClassId == item.ClassId)
                    .ToList();
                foreach (var element in checkBusyClass)
                {
                    foreach (var info in classInfo)
                    {
                        if (element.Slot == info.Slot && element.Day.Date.CompareTo(info.Day.Date) == 0)
                        {
                            message =
                                $"Class you want to add attendee to have a duplicated study time with another class that attendee is studying" +
                                $"\nAt ClassId: {item.ClassId} " +
                                $"\nWith schedule: {element} " +
                                $"\nPlease concern about sorting the schedule of class to resovle it " +
                                $"\nAttendee will still be add to Class ";
                        }
                    }
                }
            }

            var result = await _attendanceRepository.AddAsync(_mapper.Map<Attendance>(new AttendanceModel()
            {
                ClassId = classId,
                TraineeId = attendeeId,
                Date = DateTime.Now.Date,
                Status = "Active"
            }));
            await _attendanceRepository.Commit();

            if (message == null)
            {
                message = "Attendee added successfully";
            }

            return new AddAttendeeResponse()
            {
                attendance = _mapper.Map<AttendanceModel>(result),
                message = message
            };
        }

        public async Task<List<int>?> GetAllSyllabusIDsWithClassId(int classId)
        {
            var chosenClass = await _classRepository.GetByIdAsync(classId);
            var syllabusIds = _program_syllabus.GetAll().Where(x => x.TrainingProgramId == chosenClass.ProgramId)
                .ToList();
            return syllabusIds.IsNullOrEmpty() ? null : syllabusIds.Select(x => x.SyllabusId).ToList();
        }

        public async Task<string> DeleteClass(int classId)
        {
            var deletedClass = await _classRepository.GetByIdAsync(classId);

            if (await _classRepository.GetByIdAsync(classId) == null)
            {
                throw new NotFoundException("The class to be delete does not existed");
            }

            if (!_attendanceRepository
                .GetAll()
                .Where(x => x.ClassId == classId && x.Status == "Active")
                .ToList()
                .IsNullOrEmpty() 
                )
            {
                throw new Exception("Class still have student studying");
            }
            if (!_classTrainingUnitRepository
                .GetAll()
                .Where(x => x.ClassId == classId && x.Day.CompareTo(DateTime.Now) > 0)
                .ToList()
                .IsNullOrEmpty()
                )
            {
                throw new Exception("Class's schedule is not empty");
            }

            try
            {
                _classRepository.Remove(classId);
                await _classRepository.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Some thing went wrong");
            }

            var message = "Class delete successfully";

            return message;
        }

        public async Task<IEnumerable<DayScheduleModel>> SearchTrainingCalendar(string? keyword, string? roomId, DateTime? startDate, DateTime? endDate, string? shift, string? status, string? FSU, int? trainerId)
        {

            List<DateTime> days = null;
            if (startDate == null && endDate == null)
            {
                startDate = startDate ?? await _classTrainingUnitRepository.GetAll().MinAsync(c => c.Day.Date);
                endDate = endDate ?? await _classTrainingUnitRepository.GetAll().MaxAsync(c => c.Day.Date);
            }
            if (startDate == null)
                startDate = await _classTrainingUnitRepository.GetAll().MinAsync(c => c.Day.Date);
            else if (endDate == null)
                endDate = await _classTrainingUnitRepository.GetAll().MaxAsync(c => c.Day.Date);
            else if (endDate < startDate)
                throw new Exception("Enddate must be after Startdate!");
            try
            {
                days = await _classTrainingUnitRepository
                  .GetAll()
                  .Where(x => (x.Day.Date >= startDate && x.Day.Date <= endDate))
                  .Select(c => c.Day.Date)
                  .Distinct()
                  .ToListAsync();
                var daySchedules = new List<DayScheduleModel>();

                foreach (var day in days)
                {
                    List<Class_TrainingUnit> classesByDay = await _classTrainingUnitRepository
                                                                    .GetAll()
                                                                    .Include(x => x.Class)
                                                                    .Where(x => x.Day.Date == day)
                                                                    .ToListAsync();

                    List<string> keywords = new List<string>();
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        keywords = keyword.Split(',').Select(k => k.ToLower().Trim()).ToList();
                        classesByDay = classesByDay.Where(c => keywords.Any(k => c.Class.ClassName.ToLower().Contains(k)/* || c.Class.ClassCode.ToLower().Contains(k)*/)).ToList(); //class code is null
                    }

                    List<int> roomIds = new List<int>();
                    if (!string.IsNullOrEmpty(roomId))
                    {
                        var roomIdsStrings = roomId.Split(',').Select(k => k.ToLower().Trim());
                        foreach (var roomIdString in roomIdsStrings)
                        {
                            if (int.TryParse(roomIdString, out int roomIdInt))
                                roomIds.Add(roomIdInt);
                            else
                                throw new Exception($"Invalid room ID: {roomIdString}! Cannot be string.");
                        }
                        classesByDay = classesByDay.Where(c => roomIds.Any(r => c.RoomId.Equals(r))).ToList();
                    }

                    List<ClassTime> shifts = new List<ClassTime>();
                    if (!string.IsNullOrEmpty(shift))
                    {
                        var shiftStrings = shift.Split(',').Select(k => k.ToLower().Trim());
                        foreach (var shiftString in shiftStrings)
                        {
                            if (Enum.TryParse(shiftString, out ClassTime shiftEnum))
                                shifts.Add(shiftEnum);
                            else
                                throw new Exception($"Invalid shift: {shiftString}! Cannot be string.");
                        }
                        classesByDay = classesByDay.Where(c => shifts.Contains(c.Day.Hour < 12 ? ClassTime.Morning : c.Day.Hour < 18 ? ClassTime.Afternoon : ClassTime.Evening)).ToList();
                    }


                    List<string> statuses = new List<string>();
                    if (!string.IsNullOrEmpty(status))
                    {
                        statuses = status.Split(',').Select(s => s.ToLower().Trim()).ToList();
                        classesByDay = classesByDay.Where(c => statuses.Contains(c.Class.Status.ToLower())).ToList();
                    }

                    //if (!string.IsNullOrEmpty(FSU))
                    //{
                    //    classesByDay = classesByDay.Where(c => c.Class.Instructor.FSU == FSU).ToList();   // FSU null
                    //}

                    if (trainerId.HasValue)
                    {
                        int userRole = await _userRepository.FindByCondition(x => x.UserId == (int)trainerId).Select(x => x.RoleID).SingleOrDefaultAsync();
                        if (userRole == 3 || userRole == 2)
                        {
                            classesByDay = classesByDay.Where(c => c.TrainerId == trainerId).ToList();
                        }
                        else if (userRole == 4)
                        {
                            var getClassByTrainee = await _enrollmentRepository.GetAll().Include(x => x.Class).Where(x => x.TraineeId == trainerId).Select(x => x.ClassId).ToListAsync();
                            classesByDay = classesByDay.Where(x => getClassByTrainee.Contains(x.ClassId) && x.Day.Date == day).ToList();
                        }
                        else
                            throw new Exception("You don't have calendar");
                    }

                    var daySchedule = new DayScheduleModel
                    {
                        Day = day,
                        ClassTimes = new List<ClassTimeModel1>()
                    };

                    var morningClasses = new List<ClassInfoModel>();
                    var afternoonClasses = new List<ClassInfoModel>();
                    var eveningClasses = new List<ClassInfoModel>();

                    foreach (var classByDay in classesByDay)
                    {
                        var hour = classByDay.Day.Hour;

                        if (hour < 12)
                        {
                            morningClasses.Add(new ClassInfoModel
                            {
                                ClassID = classByDay.Class.ClassId,
                                ClassName = classByDay.Class.ClassName,
                                Slot = classByDay.Slot
                            });
                        }
                        else if (hour < 18)
                        {
                            afternoonClasses.Add(new ClassInfoModel
                            {
                                ClassID = classByDay.Class.ClassId,
                                ClassName = classByDay.Class.ClassName,
                                Slot = classByDay.Slot
                            });
                        }
                        else
                        {
                            eveningClasses.Add(new ClassInfoModel
                            {
                                ClassID = classByDay.Class.ClassId,
                                ClassName = classByDay.Class.ClassName,
                                Slot = classByDay.Slot
                            });
                        }
                    }

                    daySchedule.ClassTimes.Add(new ClassTimeModel1
                    {
                        Time = ClassTime.Morning,
                        Classes = morningClasses
                    });

                    daySchedule.ClassTimes.Add(new ClassTimeModel1
                    {
                        Time = ClassTime.Afternoon,
                        Classes = afternoonClasses
                    });

                    daySchedule.ClassTimes.Add(new ClassTimeModel1
                    {
                        Time = ClassTime.Evening,
                        Classes = eveningClasses
                    });

                    daySchedules.Add(daySchedule);
                }
                bool anyClassesFound = daySchedules.Any(d => d.ClassTimes.Any(classTime => classTime.Classes.Any()));

                if (!anyClassesFound)
                {
                    throw new NotFoundException("There's no record matching with your filter");
                }

                return daySchedules;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdateTrainingCalendar(int classTrainingUnitId, bool applyToAll, ClassTime shift)
        {
            var classTrainingUnit = await _classTrainingUnitRepository.GetByIdAsync(classTrainingUnitId);
            if (classTrainingUnit == null)
            {
                throw new NotFoundException("Class training unit not found");
            }

            var classId = classTrainingUnit.ClassId;
            var classTrainingUnits = applyToAll
                ? await _classTrainingUnitRepository.GetAll().Where(x => x.ClassId == classId).ToListAsync()
                : new List<Class_TrainingUnit> { classTrainingUnit };

            foreach (var unit in classTrainingUnits)
            {
                var newTime = unit.Day.Date;
                switch (shift)
                {
                    case ClassTime.Morning:
                        newTime = newTime.AddHours(new Random().Next(8, 12));
                        break;
                    case ClassTime.Afternoon:
                        newTime = newTime.AddHours(new Random().Next(13, 17));
                        break;
                    case ClassTime.Evening:
                        newTime = newTime.AddHours(new Random().Next(18, 22));
                        break;
                    default:
                        throw new Exception("Invalid shift value. Must be 0, 1, 2");
                }

                if (newTime.TimeOfDay >= unit.Day.TimeOfDay && newTime.TimeOfDay < unit.Day.TimeOfDay.Add(TimeSpan.FromHours(4)))
                {
                    throw new Exception("Error: Shift overlaps with an existing time.");
                }

                unit.Day = newTime;
                _classTrainingUnitRepository.Update(unit);
            }

            await _classTrainingUnitRepository.Commit();

            var message = "Training calendar updated successfully";
            return message;
        }
    }
}
