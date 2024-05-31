using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;

namespace Mock_Project_Net03.Services;

public class RoomService
{
    private readonly IRepository<Room, int> _roomRepository;
    private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository;
    private readonly IRepository<Class, int> _classRepository;
    private readonly IRepository<Semester, int> _semesterRepository;
    

    public RoomService(IRepository<Room, int> roomRepository, IRepository<Class_TrainingUnit, int> classTrainingUnitRepository,
        IRepository<Class,int> classRepository,IRepository<Semester, int> semesterRepository)
    {
        _roomRepository = roomRepository;
        _classTrainingUnitRepository = classTrainingUnitRepository;
        _classRepository = classRepository;
        _semesterRepository = semesterRepository;
    }
    
    public async Task<List<Room>> GetAllRooms()
    {
        return _roomRepository.GetAll().ToList();
    }

    public class FreeRoom
    {
        public DateTime Date { get; set; }
        public IEnumerable<FreeRoomSlot> Slots { get; set; } = new List<FreeRoomSlot>();
    }

    public class FreeRoomSlot
    {
        public int Slot { get; set; }
        public bool IsFree { get; set; }
    }

    public class ScheduleInfo
    {
        public int TrainingProgramUnitId { get; set; }

        public int ClassId { get; set; }

        public int TrainerId { get; set; }

        public int RoomId { get; set; }

        public int Slot { get; set; }

        public DateTime Day { get; set; }
        
        public int SemesterID { get; set; }
    }

    public async Task<List<FreeRoom>> CheckFreeSlots(int id, DateTime startDate, DateTime endDate)
    {
        var room = _roomRepository.FirstOrDefault(x => x.RoomId == id);
        if (room == null)
        {
            throw new NotFoundException("Room not found");
        }
        var freeRooms = new List<FreeRoom>();
        var dates = _classTrainingUnitRepository
            .FindByCondition(x => x.RoomId == room.RoomId).ToList();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var dateToCheck = date;
            var freeRoomSlots = new List<FreeRoomSlot>();
            var slots = dates.Where(x => x.Day.Date == dateToCheck.Date).ToList();
            for (var slot = 1; slot <= 3; slot++)
            {
                var isSlotFree = slots.All(x => x.Slot != slot);
                freeRoomSlots.Add(new FreeRoomSlot { Slot = slot, IsFree = isSlotFree });
            }
            freeRooms.Add(new FreeRoom { Date = date, Slots = freeRoomSlots});
        }
        return freeRooms;
    }

    public bool IsAvailableRoom(int roomId, int slot,DateTime date)
    {
        var trainingUnit = _classTrainingUnitRepository.FindByCondition(tu =>
            tu.RoomId == roomId && tu.Slot == slot).ToList();
        foreach (var unit in trainingUnit)
        {
            if (unit.Day.ToString("dd-MM-yyyy").Equals(date.ToString("dd-MM-yyyy")))
            {
                return false;
            }
        }
        return true;
    }

    public async Task<bool> AddScheduleForClass(ScheduleInfo info)
    {
        var schedule = _classTrainingUnitRepository.AddAsync(new Class_TrainingUnit()
        {
            TrainingProgramUnitId = info.TrainingProgramUnitId,
            TrainerId = info.TrainerId,
            ClassId = info.ClassId,
            RoomId = info.RoomId,
            Slot = info.Slot,
            Day = info.Day
        });
        var existedClass = await _classRepository.GetByIdAsync(info.ClassId);
        var semester = await _semesterRepository.GetByIdAsync(info.SemesterID);
        existedClass.SemesterId = semester.SemesterID;
        existedClass.Semester = semester;
        _classRepository.Update(existedClass);
        return await _classTrainingUnitRepository.Commit() >0;
    }

    public bool HasSchedule(int classId)
    {
        bool result = _classTrainingUnitRepository.FindByCondition(x => x.ClassId == classId).FirstOrDefault() is not null
            ? true
            : false;
        return result;
    }
}