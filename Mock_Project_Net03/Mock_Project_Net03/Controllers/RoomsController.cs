using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Services;
using OfficeOpenXml.Packaging.Ionic.Zip;

namespace Mock_Project_Net03.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly RoomService _roomService;
    private readonly UserService _userService;
    private readonly PermissionService _permissionService;
    private readonly TrainingProgramServices _trainingProgramServices;
    private readonly ClassService _classService;
    private readonly SyllabusService _syllabusService;
    private readonly SemesterService _semesterService;

    public RoomsController(RoomService roomService, UserService userService, PermissionService permissionService
        , TrainingProgramServices trainingProgramService, ClassService classService, SyllabusService syllabusService,
        SemesterService semesterService)
    {
        _roomService = roomService;
        _userService = userService;
        _permissionService = permissionService;
        _trainingProgramServices = trainingProgramService;
        _classService = classService;
        _syllabusService = syllabusService;
        _semesterService = semesterService;
    }

    [Authorize]
    [HttpGet("GetAllRooms")]
    public async Task<ActionResult<List<RoomModel>>> GetAllRooms()
    {
        var rooms = await _roomService.GetAllRooms();
        return Ok(ApiResult<List<RoomModel>>.Succeed(rooms.Select(x => new RoomModel
        {
            RoomId = x.RoomId,
            Name = x.Name,
            Description = x.Description
        }).ToList()));
    }

    [Authorize(Roles = "Super Admin, Admin, Instructor")]
    [HttpPost("{roomId:int}/check-free-slots")]
    public async Task<IActionResult> CheckFreeSlots([FromRoute] int roomId, [FromBody] CheckFreeSlotsRequest req)
    {
        Request.Headers.TryGetValue("Authorization", out var token);
        token = token.ToString().Split()[1];
        var currentUser = await _userService.GetUserInToken(token);
        var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
        // if (!permission.SyllabusAccess.Equals("Modify") && !permission.SyllabusAccess.Equals("Full access"))
        // {
        //     return Forbid("This account does not have permission");
        // }

        var freeRooms = await _roomService.CheckFreeSlots(roomId, req.StartDate, req.EndDate);
        return Ok(ApiResult<CheckFreeSlotsResponse>.Succeed(new CheckFreeSlotsResponse
        {
            Dates = freeRooms,
        }));
    }

    [Authorize(Roles = "Super Admin, Admin, Instructor")]
    [HttpPost("/create-class-schedule")]
    public async Task<IActionResult> CreateScheduleForClass([FromBody] CreateClassScheduleRequest req)
    {
        Request.Headers.TryGetValue("Authorization", out var token);
        token = token.ToString().Split()[1];
        var currentUser = await _userService.GetUserInToken(token);
        var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
        // if (!permission.SyllabusAccess.Equals("Modify") && !permission.SyllabusAccess.Equals("Full access"))
        // {
        //     return Forbid("This account does not have permission");
        // }

        var existedClass = await _classService.GetClassById(req.ClassId);
        if (existedClass is null)
        {
            throw new BadRequestException("This class does not exist");
        }

        if (_roomService.HasSchedule(req.ClassId))
        {
            throw new BadRequestException("This class already has its schedule");
        }

        var learningDates = new List<DateTime>();
        foreach (var detail in req.ScheduleDetails)
        {
            var semester = await _semesterService.GetSemesterById(req.SemesterId);
            if (detail.Date < semester.SemesterStartDate)
            {
                throw new BadRequestException("This class begin before semester");
            }
            else if (detail.Date > semester.SemesterStartDate.AddDays(6))
            {
                throw new BadRequestException("Class must start in the first week of the semester");
            }
        }

        var dic = new Dictionary<DateAndSlot, int>();
        var syllabusIds = await _classService.GetAllSyllabusIDsWithClassId(req.ClassId);
        if (syllabusIds is null)
        {
            throw new BadRequestException("This Training Program of this class do not have any content");
        }

        var errSyllabusIds = new List<int>();
        foreach (var id in req.ScheduleDetails.Select(detail => detail.SyllabusId))
        {
            if (!syllabusIds.Contains(id))
            {
                errSyllabusIds.Add(id);
            }
        }

        if (errSyllabusIds.Any())
        {
            var errMessage = "";
            foreach (var errId in errSyllabusIds)
            {
                errMessage += $"{errId.ToString()} ";
            }
            throw new BadRequestException(
                $"These SyllabusId is not in the training program: {errMessage}");
        }

        foreach (var dateAndSlot in req.ScheduleDetails.Select(scheduleDetail => new DateAndSlot
                 {
                     Date = scheduleDetail.Date,
                     Slot = scheduleDetail.Slot,
                 }))
        {
            if (dic.TryGetValue(dateAndSlot, out _))
            {
                throw new BadRequestException("Duplicated class is existed");
            }

            dic.Add(dateAndSlot, 1);
        }

        var existedRoomAndSlot = new List<DateAndSlot>();
        foreach (var detail in req.ScheduleDetails)
        {
            var units = _syllabusService.GetAllTrainingUnitsBySyllabusId(detail.SyllabusId);
            DateTime tempDate = detail.Date;
            foreach (var unit in units)
            {
                if (!_roomService.IsAvailableRoom(detail.RoomId, detail.Slot, tempDate))
                {
                    existedRoomAndSlot.Add(new DateAndSlot()
                    {
                        Slot = detail.Slot,
                        Date = detail.Date
                    });
                    break;
                }
                await _roomService.AddScheduleForClass(new RoomService.ScheduleInfo()
                {
                    Slot = detail.Slot,
                    RoomId = detail.RoomId,
                    ClassId = req.ClassId,
                    Day = tempDate,
                    TrainerId = detail.TrainerId,
                    TrainingProgramUnitId = unit.UnitId,
                    SemesterID = req.SemesterId
                });
                tempDate = tempDate.AddDays(7);
            }
        }
        if (!existedRoomAndSlot.Any())
            return Ok(ApiResult<CreateScheduleForClassResponse>.Succeed(new CreateScheduleForClassResponse()
            {
                Message = "Create schedule successfully"
            }));
        var details = string.Join(", ", existedRoomAndSlot.Select(item => item.ToString()));
        throw new BadRequestException($"The room has been used in Date and Slot:{details}");

    }

    private struct DateAndSlot
    {
        public DateTime Date { get; set; }
        public int Slot { get; set; }
        public override string ToString()
        {
            return $"Slot: {this.Slot}, Date:{this.Date.ToString("dd-MM-yyyy")}";
        }
    }
}