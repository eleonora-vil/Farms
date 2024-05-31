using Mock_Project_Net03.Services;

namespace Mock_Project_Net03.Common.Payloads.Responses;

public class CheckFreeSlotsResponse
{
    public IEnumerable<RoomService.FreeRoom> Dates { get; set; } = new List<RoomService.FreeRoom>();
}