using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using System.Linq;

namespace Mock_Project_Net03.Services
{
    public class ClassService
    {
        private readonly IRepository<Class, int> _classRepository;
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository;
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<Attendance, int> _attendanceRepository;
        private readonly IMapper _mapper;

        public ClassService(IRepository<Class, int> classRepository, IRepository<Attendance, int> attendanceRepository,
            IRepository<Class_TrainingUnit, int> classTrainingUnitRepository, IRepository<User, int> userRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _attendanceRepository = attendanceRepository;
            _classTrainingUnitRepository = classTrainingUnitRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ClassModel> GetClassById(int id)
        {
            var classEntity = await _classRepository
                .FindByCondition(x => x.ClassId == id)
                .Include(x => x.Instructor)
                .Include(x => x.Program)
                .FirstOrDefaultAsync();

            return _mapper.Map<ClassModel>(classEntity);
        }

        public async Task<(IEnumerable<ClassModel>, int totalPages)> GetAllClass(int pageNumber, int pageSize)
        {
            var totalItems = await _classRepository.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var classList = await _classRepository
               .GetAll()
               .Include(x => x.Instructor)
               .Include(x => x.Program)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

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
        public async Task<IEnumerable<ClassModel>> GetAllClassWithFilter(
            IEnumerable<ClassModel> classModels,
            string? KeyWord,
            //            string? ClassLocation,
            string? ClassTime,
            string? Status,
            DateTime TimeFrom,
            DateTime TimeTo,
            string? FSU,
            int TrainerId
            )
        {
            if (TimeTo.Equals(DateTime.MinValue))
            {
                TimeTo = DateTime.MaxValue;
            }

            if (TimeFrom.CompareTo(TimeTo) > 0)
            {
                throw new BadRequestException("Date from must be before date to");
            }


            var listClassModelWithFilter = classModels;

            if (!string.IsNullOrEmpty(KeyWord))
            {
                listClassModelWithFilter = listClassModelWithFilter
                    .Where(x => x.ClassName.ToLower().Contains(KeyWord.ToLower())); // class name match keyword
            }

            if (!TimeFrom.Equals(DateTime.MinValue))
            {
                listClassModelWithFilter = listClassModelWithFilter
                    .Where(x => x.EndDate.CompareTo(TimeFrom) > 0);
                // class start time < timeFrom + end time > timeTo => class still active in filtered date
            }

            if (!TimeTo.Equals(DateTime.MaxValue))
            {
                listClassModelWithFilter = listClassModelWithFilter
                    .Where(x => x.StartDate.CompareTo(TimeTo) < 0);
                // class start time < timeFrom + end time > timeTo => class still active in filtered date
            }

            if (TrainerId != 0)
            {
                listClassModelWithFilter = listClassModelWithFilter
                    .Where(x => x.InstructorId == TrainerId); // compare trainer to instructor's username ?
            }

            if (!string.IsNullOrEmpty(FSU))
            {
                //            listClassModelWithFilter = listClassModelWithFilter
                //.Where(x => x. ? =  FSU); FSU là FSU cua trainer a
            }
            /*               
                       if (!string.IsNullOrEmpty(ClassLocation) )
                       {
                           //listClassModelWithFilter = listClassModelWithFilter.Where(x =>  ClassLocation.Contains(x.location));
                       }
           */
            if (!string.IsNullOrEmpty(ClassTime))
            {
                listClassModelWithFilter = listClassModelWithFilter
                    .Where(x => ClassTime.ToLower().Contains(x.Time.ToLower()));
            }

            if (!string.IsNullOrEmpty(Status))
            {
                listClassModelWithFilter = listClassModelWithFilter
                    .Where(x => Status.ToLower().Contains(x.Status.ToLower()));
            }

            if (!listClassModelWithFilter.Any())
            {
                throw new NotFoundException("There's no record matching with your keyword");
            }
            return listClassModelWithFilter.ToList();
        }
        public async Task<GetClassDetailResponse> GetClassDetail(int id)
        {
            GetClassDetailResponse response = new GetClassDetailResponse();

            var classEntity = await _classRepository
                .FindByCondition(x => x.ClassId == id)
                .Include(x => x.Instructor)
                .Include(x => x.Program)
                .FirstOrDefaultAsync();

            if (classEntity is null)
            {
                return null;
            }

            var attendees = await _attendanceRepository
                .GetAll()
                .Where(x => x.ClassId == classEntity.ClassId)
                .ToListAsync();

            response.Class = _mapper.Map<ClassModel>(classEntity);
            response.attendees = attendees;

            return response;
        }
        public async Task<List<UserModel>> SearchFreeInstructors(CheckFreeIntructorModel req)
        {
            try
            {
                var ins = _classTrainingUnitRepository.FindByCondition(i => i.Slot == req.Slot && i.Day.Date == req.Day.Date)
                                                        .Select(i => i.TrainerId);

                var instructorList = _userRepository.GetAll().Where(u => !ins.Contains(u.UserId)).ToList();

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
                throw new BadRequestException("ClassI does not exist!");
            }
            searchClass.Status = processStatus;
            _classRepository.Update(searchClass);
            var result = await _classRepository.Commit();
            if (result >0)
            {
                var viewClass = await GetClassById(id);
                return viewClass;
            }
            return null;

        }
    }
}
