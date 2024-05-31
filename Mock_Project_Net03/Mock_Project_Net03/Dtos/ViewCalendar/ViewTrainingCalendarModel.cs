using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Dtos.ViewCalendar
{
    public class ViewTrainingCalendarModel
    {
        public class ClassInfoModel
        {
            public int ClassID {  get; set; }
            public string ClassName { get; set; } //  lớp học
            public int Slot { get; set; } // Slot
        }

        public class ClassTimeModel1
        {
            public ClassTime Time { get; set; } // Thời gian buổi học
            public List<ClassInfoModel> Classes { get; set; } // Danh sách lớp học
        }

        public class DayScheduleModel
        {
            public DateTime Day { get; set; } // Ngày trong tuần
            public List<ClassTimeModel1> ClassTimes { get; set; } // Danh sách các buổi học trong ngày
        }
    }
    }
