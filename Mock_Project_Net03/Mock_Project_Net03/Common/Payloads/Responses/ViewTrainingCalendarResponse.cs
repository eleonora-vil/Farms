using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Dtos.ViewCalendar;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class ViewTrainingCalendarResponse
    {
        public IEnumerable<ViewTrainingCalendarModel.DayScheduleModel> viewTrainingCalendarModel { get; set; }
    }
}
