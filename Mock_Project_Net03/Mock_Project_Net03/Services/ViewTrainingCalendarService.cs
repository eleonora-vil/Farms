using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Dtos.ViewCalendar;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using System;
using static Mock_Project_Net03.Dtos.ViewCalendar.ViewTrainingCalendarModel;

namespace Mock_Project_Net03.Services
{
    public class ViewTrainingCalendarService
    {
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository;
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<Enrollment, int> _enrollmentRepository;
        private readonly IRepository<Class, int> _classRepository;
        private readonly IMapper _mapper;
        public ViewTrainingCalendarService(IRepository<Class_TrainingUnit, int> classTrainingUnitRepository, IRepository<Class, int> classRepository, IRepository<User, int> userRepository, IRepository<Enrollment, int> enrollmentRepository,  IMapper mapper)
        {
            _userRepository = userRepository;
            _enrollmentRepository = enrollmentRepository;

            _classTrainingUnitRepository = classTrainingUnitRepository;
            _classRepository = classRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DayScheduleModel>> GetDay(DateTime startDay, DateTime endDate, int? userID)
        {
            
            List<DateTime> days = null;
            int userRole = 0;
            try
            {


                if (userID != null)
                {
                    userRole = await _userRepository.FindByCondition(x => x.UserId == (int)userID).Select(x => x.RoleID).SingleOrDefaultAsync();
                    if (userRole == 3 || userRole == 2)
                    {
                        days = await _classTrainingUnitRepository
                       .GetAll()
                       .Where(x => (x.TrainerId == userID) && (x.Day.Date >= startDay.Date && x.Day.Date <= endDate.Date))
                       .Include(x => x.Class)
                       .Select(c => c.Day.Date)
                       .Distinct()
                       .ToListAsync();
                    }
                    else if (userRole == 4)
                    {
                        var getClassByTrainee = await _enrollmentRepository.GetAll().Include(x => x.Class).Where(x => x.TraineeId == userID).Select(x => x.ClassId).ToListAsync();
                        days = await _classTrainingUnitRepository
                       .GetAll()
                       .Where(x => getClassByTrainee.Contains(x.ClassId) && (x.Day.Date >= startDay.Date && x.Day.Date <= endDate.Date))
                       .Include(x => x.Class)
                       .Select(c => c.Day.Date)
                       .Distinct()
                       .ToListAsync();

                    }
                    else
                    {
                        throw new Exception("You have no permission to view TrainingProgram calendar");
                    }
                    
                }
                else
                {
                    days = await _classTrainingUnitRepository
                      .GetAll()
                      .Where(x => (x.Day.Date >= startDay.Date && x.Day.Date <= endDate.Date))
                      .Select(c => c.Day.Date)
                      .Distinct()
                      .ToListAsync();
                }
                var daySchedules = new List<DayScheduleModel>();

                foreach (var day in days)
                {
                    List<Class_TrainingUnit> classesByDay = null;
                    if (userID != null)
                    {
                        if (userRole == 3 || userRole == 2)
                        {
                            classesByDay = await _classTrainingUnitRepository
                           .GetAll()
                           .Include(x => x.Class)
                           .Where(c => c.Day.Date == day && c.TrainerId == userID)
                           .ToListAsync();
                        }
                        else if (userRole == 4)
                        {
                            var getClassByTrainee = await _enrollmentRepository.GetAll().Include(x => x.Class).Where(x => x.TraineeId == userID).Select(x => x.ClassId).ToListAsync();
                            classesByDay = await _classTrainingUnitRepository
                           .GetAll()
                           .Include(x => x.Class)
                           .Where(x => getClassByTrainee.Contains(x.ClassId) && x.Day.Date == day)
                           .ToListAsync();
                        }
                    }

                    else
                    {
                        classesByDay = await _classTrainingUnitRepository
                                       .GetAll()
                                       .Include(x => x.Class)
                                       .Where(x => x.Day.Date == day)
                                       .ToListAsync();
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

                return daySchedules;
            }
            catch (Exception ex)
            {
                throw new Exception("You don't have calendar", ex);
            }
        }
    }
}
