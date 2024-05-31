using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Validation;
using System.Data.OleDb;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProgramController : ControllerBase
    {
        private readonly TrainingProgramServices _trainingProgramServices;
        private readonly SyllabusService _syllabusService;
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;
        public TrainingProgramController(SyllabusService syllabusService, TrainingProgramServices trainingProgramServices
            , UserService userService, PermissionService permissionService)
        {
            _trainingProgramServices = trainingProgramServices
                ?? throw new ArgumentNullException(nameof(trainingProgramServices));
            _userService = userService
                ?? throw new ArgumentNullException(nameof(userService));
            _permissionService = permissionService
                ?? throw new ArgumentNullException(nameof(permissionService));
            _syllabusService = syllabusService;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetListTrainingPrograms()
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            if (!permission.UserAccess.Equals("View") && !permission.UserAccess.Equals("Full access"))
            {
                throw new BadRequestException("This account do not have permission");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new BadRequestException("Authorization header is missing or invalid.");
            }
            var result = await _trainingProgramServices.GetAllTrainingPrograms();
            return Ok(ApiResult<TrainingProgramResponse>.Succeed(new TrainingProgramResponse
            {
                TrainingPrograms = result,
            }));
        }
        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Super Admin ,Admin")]
        public async Task<IActionResult> CreateTrainingProgram([FromBody] CreateTrainingProgramRequest req)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            //var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            //if (!permission..Equals("Create") && !permission.UserAccess.Equals("Full access"))
            //{
            //    throw new BadRequestException("This account do not have permission");
            //}
            var program = req.ToTrainingProgram();
            program.CreateBy = currentUser.UserName;
            List<SyllabusModel> syllabusModels = new List<SyllabusModel>();
            foreach (int sid in req.SyllabusID)
            {
                var syllabus = await _syllabusService.GetSimpleSyllabusById(sid);
                if (syllabus != null)
                {
                    syllabusModels.Add(syllabus);
                }
                else
                {
                    throw new BadRequestException($"Syllsbus Id {sid} does not existed ");
                }
            }
            var result = await _trainingProgramServices.CreateTrainingProgram(program, syllabusModels);
            if (result is not null)
            {
                return Ok(ApiResult<CreateTrainingProgramResponse>.Succeed(new CreateTrainingProgramResponse()
                {
                    Program = program,
                }));
            }
            else
            {
                throw new BadHttpRequestException("Something went wrong");
            }

        }

        //Khi ấn cancel
        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> DeleteTrainingProgramByID([FromRoute] int id)
        {
            var result = await _trainingProgramServices.DeleteTrainingProgram(id);
            if (result)
            {
                return Ok(ApiResult<DeleteTrainingProgramResponse>.Succeed(new DeleteTrainingProgramResponse()
                {
                    Message = "Delete Successfully"
                }));
            }
            else
            {
                throw new BadRequestException("Something went wrong");
            }
        }
        [HttpPut("addSyll/{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> AddSyllabusestoTrainingProgram([FromBody] AddSyllabusToTrainingProgramRequest req, [FromRoute] int id)
        {
            List<SyllabusModel> syllabusModels = new List<SyllabusModel>();
            foreach (int sid in req.SyllabusID)
            {
                var syllabus = await _syllabusService.GetSimpleSyllabusById(sid);
                if (syllabus != null)
                {
                    syllabusModels.Add(syllabus);
                }
                else
                {
                    throw new BadRequestException($"Syllsbus Id {sid} does not existed ");
                }
            }
            bool result = await _trainingProgramServices.AddSyllabusToTrainingProgram(syllabusModels, req.Duration, id);
            if (!result)
            {
                throw new BadRequestException("Something went wrong");
            }

            return Ok(ApiResult<AddSyllabusToTrainingProgramResponse>.Succeed(new AddSyllabusToTrainingProgramResponse()
            {
                Message = "Create Successfully"
            }));
        }

        [HttpPost("importCSV")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> ImportTrainingProgramByCSV([FromForm] ImportTrainingProgramRequest req)
        {
            if (req.File == null || req.File.Length == 0)
            {
                return BadRequest("File is not selected or empty");
            }

            // Validate the file using the FileImportValidator
            //var validator = new FileImportValidator();
            //var validationResult = await file.ValidateAsync();
            //if (validationResult != null)
            //{
            //    return BadRequest(validationResult);
            //}

            // Proceed with CSV file processing since it's valid
            var requests = ProcessCSV(req.File);
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            foreach (var program in requests)
            {
                var trainingProgram = _trainingProgramServices.GetTrainingProgramByName(program.ProgramName);
                if (trainingProgram is null || req.DuplicateHandle.ToLower().Equals("allow"))
                {
                    TrainingProgramModel newProgram = new TrainingProgramModel()
                    {
                        ProgramName = program.ProgramName,
                        Description = program.Information
                    };
                    List<SyllabusModel> syllabusModels = new List<SyllabusModel>();
                    foreach (var code in program.SyllabusCode)
                    {
                        if (code.IsNullOrEmpty())
                        {
                            throw new BadRequestException("Can not get an Empty Syllabus for the Training Program");
                        }
                        var syllabusModel = _syllabusService.GetSyllabusByCode(code);
                        syllabusModels.Add(syllabusModel);
                    }
                    newProgram.CreateBy = currentUser.UserName;
                    await _trainingProgramServices.CreateTrainingProgram(newProgram, syllabusModels);
                }
                else if (req.DuplicateHandle.ToLower().Equals("replace"))
                {
                    await _trainingProgramServices.DeleteTrainingProgram(trainingProgram.ProgramId);
                    trainingProgram.Description = program.Information;
                    trainingProgram.ProgramName = program.ProgramName;
                    List<SyllabusModel> syllabusModels = new List<SyllabusModel>();
                    foreach (var code in program.SyllabusCode)
                    {
                        var syllabusModel = _syllabusService.GetSyllabusByCode(code);
                        syllabusModels.Add(syllabusModel);
                    }
                    await _trainingProgramServices.CreateTrainingProgram(trainingProgram, syllabusModels);
                }
            }

            return Ok(ApiResult<ImportTrainingProgramResponse>.Succeed(new ImportTrainingProgramResponse()
            {
                Message = "Import Success"
            }));
        }
        private List<ImportTrainingProgramData> ProcessCSV(IFormFile file)
        {
            List<ImportTrainingProgramData> requests = new List<ImportTrainingProgramData>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                // Read the header line
                var headerLine = reader.ReadLine();
                if (headerLine != null && headerLine.Equals("Name, Information,List Syllabus"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = SplitCSVLine(line);

                        if (values.Length >= 3)
                        {
                            var request = new ImportTrainingProgramData
                            {
                                ProgramName = values[0],
                                Information = values[1],
                                SyllabusCode = values[2].Split(',').Select(s => s.Trim()).ToList()
                            };

                            // Check for null or empty values
                            if (!string.IsNullOrEmpty(request.ProgramName) && !string.IsNullOrEmpty(request.Information))
                            {
                                requests.Add(request);
                            }
                            else
                            {
                                throw new BadRequestException("Warning: Null or empty values found. Skipping entry.");
                            }
                        }
                    }
                }
                else
                {
                    throw new BadRequestException("Error: Header format incorrect.");
                }
            }
            return requests;
        }
        private string[] SplitCSVLine(string line)
        {
            List<string> values = new List<string>();
            StringBuilder currentValue = new StringBuilder();
            bool inQuotes = false;

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(currentValue.ToString());
                    currentValue.Clear();
                }
                else
                {
                    currentValue.Append(c);
                }
            }

            values.Add(currentValue.ToString());
            return values.ToArray();
        }
        [HttpPut]
        [Route("UpdateTrainingProgram")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> updateTrainingProgram([FromBody] UpdateTrainingProgramRequest req)
        {
            var raw_data = req.ToUpdateTrainingProgram();
            var raw_syllabusIds = req.SyllabusIds;
            var syllabuses = new List<TrainingProgram_Syllabus>();
            foreach (var data in raw_syllabusIds)
            {
                var Syllabus = new TrainingProgram_Syllabus
                {
                    SyllabusId = data
                };
                syllabuses.Add(Syllabus);
            }
            raw_data.TrainingProgram_Syllabus = syllabuses;
            var result = await _trainingProgramServices.UpdateTrainingProgram(raw_data);
            var listSyllabuses = _syllabusService.GetSyllabusByTrainingProgramId(raw_data.ProgramId);
            if (result != null)
            {
                return Ok(ApiResult<UpdateTrainingProgramResponse>.Succeed(new UpdateTrainingProgramResponse
                {
                    message = $"Update Successful for ProgramId: {result.ProgramId}",
                    ProgramName = result.ProgramName,
                    Description = result.Description,
                    Status = result.Status,
                    CreateBy = result.CreateBy,
                    ListSyllabus = listSyllabuses
                    //Updated_Data = result,
                    //SyllabusIds = result.TrainingProgram_Syllabus.Select( s=> s.SyllabusId ).ToList(),
                }));

            }
            return BadRequest(ApiResult<UpdateTrainingProgramResponse>.Error(new UpdateTrainingProgramResponse
            {
                message = "Can't handle this request!"
            }));
        }

        [HttpPut]
        [Authorize(Roles = "Super Admin,Admin")]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatusProGra(int programId, string status)
        {
            if (status.ToLower().Contains("active") || status.ToLower().Contains("deactive") || status.ToLower().Contains("draft"))
            {
                var program = await _trainingProgramServices.UpdateStatusTrainingProgram(programId, status);
                if (program != null)
                {
                    var message = $"Update status successful for ProgramId: {program.ProgramId} with Status: {program.Status}";
                    return Content(message);
                }
                return BadRequest(ApiResult<UpdateTrainingProgramResponse>.Error(new UpdateTrainingProgramResponse
                {
                    message = "Can't handle this request!"
                }));
            }

            return Content("Input status must be: Active,Deactive or Draft!");

        }

        [HttpGet("getDetails/{id}")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> GetTrainingProgramDetails([FromRoute] int id)
        {
            var trainingProgram = await _trainingProgramServices.GetTrainingProgramById(id);
            if (trainingProgram is null)
            {
                throw new BadRequestException("This training program does not exist");
            }
            var listSyllabuses = _syllabusService.GetSyllabusByTrainingProgramId(id);
            return Ok(ApiResult<GetTrainingProgramDetails>.Succeed(new GetTrainingProgramDetails()
            {
                ProgramName = trainingProgram.ProgramName,
                Description = trainingProgram.Description,
                Status = trainingProgram.Status,
                CreateBy = trainingProgram.CreateBy,
                ListSyllabus = listSyllabuses
            }));
        }
        [HttpGet("search")]
        [Authorize(Roles = "Super Admin, Admin, Instructor")]
        public async Task<IActionResult> SearchTrainingPrograms(string? keyword, int pageNumber, int pageSize, string? createdBy, DateTime? startDate, DateTime? endDate, string? status)
        {
            if (keyword.IsNullOrEmpty())
            {
                keyword = " ";
            }
            Func<TrainingProgram, bool> filter = p =>
                (string.IsNullOrEmpty(createdBy) || p.CreateBy.ToLower() == createdBy.ToLower()) &&
                (!startDate.HasValue || p.StartDate.Value.Date >= startDate.Value.Date) &&
                (!endDate.HasValue || p.EndDate.Value.Date <= endDate.Value.Date) &&
                (string.IsNullOrEmpty(status) || p.Status.ToLower() == status.ToLower());
            if (pageNumber == 0) pageNumber = 1;
            if (pageSize == 0)  pageSize = int.MaxValue;
            var trainingPrograms = await _trainingProgramServices.SearchTrainingProgram(keyword, pageNumber, pageSize, filter);
            if (!trainingPrograms.Any())
            {
                return Ok(ApiResult<SearchTrainingProgramResponse>.Succeed(new SearchTrainingProgramResponse
                {
                    message = "There's no record matching with you keyword!",
                })); ;
            }
            return Ok(ApiResult<SearchTrainingProgramResponse>.Succeed(new SearchTrainingProgramResponse
            {
                message = $"Found {trainingPrograms.Count} programs!",
                list = trainingPrograms,
            })); ;

        }

    }
}
