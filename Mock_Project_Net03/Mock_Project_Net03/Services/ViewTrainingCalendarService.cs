using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Dtos.ViewCalendar;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using static Mock_Project_Net03.Dtos.ViewCalendar.ViewTrainingCalendarModel;

namespace Mock_Project_Net03.Services
{
    public class ViewTrainingCalendarService
    {
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository;
        private readonly IRepository<Class, int> _classRepository;
        private readonly IMapper _mapper;
        public ViewTrainingCalendarService(IRepository<Class_TrainingUnit, int> classTrainingUnitRepository, IRepository<Class, int> classRepository, IMapper mapper)
        {
            _classTrainingUnitRepository = classTrainingUnitRepository;
            _classRepository = classRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DayScheduleModel>> GetDay()
        {
            var days = await _classTrainingUnitRepository
                .GetAll()
                .Select(c => c.Day.Date)
                .Distinct()
                .ToListAsync();

            var daySchedules = new List<DayScheduleModel>();

            foreach (var day in days)
            {
                var classesByDay = await _classTrainingUnitRepository
                    .GetAll()
                    .Include(x => x.Class)
                    .Where(c => c.Day.Date == day)
                    .ToListAsync();

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
                            ClassName = classByDay.Class.ClassName,
                            Slot = classByDay.Slot
                        });
                    }
                    else if (hour < 18)
                    {
                        afternoonClasses.Add(new ClassInfoModel
                        {
                            ClassName = classByDay.Class.ClassName,
                            Slot = classByDay.Slot
                        });
                    }
                    else
                    {
                        eveningClasses.Add(new ClassInfoModel
                        {
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
    }
}
