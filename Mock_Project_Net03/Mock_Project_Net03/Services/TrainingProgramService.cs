using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Common.Payloads.Requests;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using Org.BouncyCastle.Ocsp;
using System.Net.WebSockets;
using Microsoft.IdentityModel.Tokens;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common;

namespace Mock_Project_Net03.Services
{
    public class TrainingProgramServices
    {
        private readonly IRepository<TrainingProgram, int> _trainPro;
        private readonly IMapper _mapper;
        private readonly IRepository<TrainingProgram_Syllabus, int> _TrainSyllabus;
        private readonly IRepository<Entities.Syllabus, int> _sy;
        private readonly IRepository<TrainingProgramUnit, int> _trainingUnit;

        public TrainingProgramServices(IRepository<TrainingProgram, int> trainPro, IMapper mapper, IRepository<TrainingProgram_Syllabus, int> trainSyllabus, IRepository<Entities.Syllabus, int> sy,
            IRepository<TrainingProgramUnit, int> trainingUnit)

        {
            _trainPro = trainPro;
            _mapper = mapper;
            _sy = sy;
            _TrainSyllabus = trainSyllabus;
            _trainingUnit = trainingUnit;
        }
        //public async Task<TrainingProgramModel> CreateTrainingProgram(TrainingProgramModel newTrainPro)
        //{
        //    var training =  _mapper.Map<TrainingProgram>(newTrainPro);
        //}

        public async Task<TrainingProgramModel> CreateTrainingProgram(TrainingProgramModel newProgram, List<SyllabusModel> syllabusModels)
        {
            var program = _mapper.Map<TrainingProgram>(newProgram);
            //var existedProgram = await _trainPro.FindByCondition(x => x.ProgramName.ToLower().Equals(newProgram.ProgramName.ToLower())).Include(tp => tp.TrainingProgram_Syllabus).FirstOrDefaultAsync();
            //if (existedProgram is not null)
            //{
            //    throw new BadRequestException("This Training Program has been existed");
            //}
            program = await _trainPro.AddAsync(program);
            var syllabusEnities = _mapper.Map<List<Entities.Syllabus>>(syllabusModels);
            foreach (var syllabus in syllabusEnities)
            {
                program.TrainingProgram_Syllabus.Add(new TrainingProgram_Syllabus()
                {
                    TrainingProgramId = program.ProgramId,
                    SyllabusId = syllabus.SyllabusId,
                    Status = "Active",
                    //TrainingProgram = trainingProgram,
                    //Syllabus = syllabus
                });
            }
            program.Status = "Active";

            int result = await _trainPro.Commit();
            if (result > 0)
            {
                newProgram.ProgramId = program.ProgramId;
                newProgram.Status = program.Status;
                return newProgram;
            }
            return null;
        }

        public async Task<bool> DeleteTrainingProgram(int programID)
        {
            var program = await _trainPro.FindByCondition(tp => tp.ProgramId == programID).Include(tp => tp.TrainingProgram_Syllabus).FirstOrDefaultAsync();
            if (program is null)
            {
                throw new BadRequestException("Can not find this Program");
            }
            program.TrainingProgram_Syllabus.Clear();
            _trainPro.Remove(programID);
            return await _trainPro.Commit() > 0;
        }
        public async Task<bool> AddSyllabusToTrainingProgram(List<SyllabusModel> syllabusModels, int duration, int programID)
        {
            var trainingProgram = _trainPro.FindByCondition(tp => tp.ProgramId == programID)
                .Include(tp => tp.TrainingProgram_Syllabus)
                .FirstOrDefault();
            if (trainingProgram is null)
            {
                throw new BadRequestException("This TrainingProgram is not found");
            }
            var syllabusEnities = _mapper.Map<List<Entities.Syllabus>>(syllabusModels);
            foreach (var syllabus in syllabusEnities)
            {
                trainingProgram.TrainingProgram_Syllabus.Add(new TrainingProgram_Syllabus()
                {
                    TrainingProgramId = programID,
                    SyllabusId = syllabus.SyllabusId,
                    //TrainingProgram = trainingProgram,
                    //Syllabus = syllabus
                });
            }
            trainingProgram.StartDate = DateTime.Now;
            trainingProgram.EndDate = DateTime.Now.AddDays(duration);
            trainingProgram.Status = "Active";
            _trainPro.Update(trainingProgram);
            return await _trainPro.Commit() > 0;
        }
        public Task<List<TrainingProgramModel>> GetAllTrainingPrograms()
        {
            var result = _trainPro.GetAll();
            var trainingPrograms = _mapper.Map<List<TrainingProgramModel>>(result);
            return Task.FromResult(trainingPrograms);
        }
        public async Task<TrainingProgramModel> UpdateTrainingProgram(TrainingProgramModel model)
        {
            var train = _mapper.Map<TrainingProgram>(model);
            //check program exist
            if (model.ProgramId <= 0)
            {
                throw new BadRequestException("ProgramId must be a positive integer!");
            }
            var programExist = _trainPro.FindByCondition(x => x.ProgramId == train.ProgramId).FirstOrDefault();
            if (programExist == null)
            {
                throw new BadRequestException("ProgramId does not exist!");
            }
            //Check program is available to update 
            // If a class is using program, program coundn't update

            //Check active
            var getCurrentProgram = await _trainPro.GetByIdAsync(train.ProgramId);
            var processCurrentProgram = getCurrentProgram.Status.ToLower();
            if (processCurrentProgram == "active")
            {
                //Check Length of Description
                if (model.Description.Length > 255)
                {
                    throw new BadRequestException("The maximum length of General Information is 255 characters!");
                }
                //Check StartDate < CreateDate
                if (model.StartDate.Date < getCurrentProgram.CreateDate.Value.Date)
                {
                    throw new BadRequestException("StartDate must be lately than or equal to CreateDate!");
                }
                //Check StartDate & EndDate
                if (model.StartDate >= model.EndDate)
                {
                    throw new BadRequestException("End Date must be lately than Start Date!");
                }
                //Check syllabus exist
                var SyllabusIdFromRequest = train.TrainingProgram_Syllabus.ToList();
                foreach (var syE in train.TrainingProgram_Syllabus.ToList())
                {
                    var check = _sy.FindByCondition(z => z.SyllabusId == syE.SyllabusId).FirstOrDefault();
                    if (check == null)
                    {
                        throw new BadRequestException($"SyllabusId: {syE.SyllabusId} does not exist!");
                    }
                }
                //Deactive syllabusid before update
                var existingsyllabuses = _TrainSyllabus.FindByCondition(s => s.TrainingProgramId == train.ProgramId)
                                                //.Select(x => x.SyllabusId)
                                                .ToList();
                foreach (var existingsyllabus in existingsyllabuses)
                {
                    existingsyllabus.Status = "Deactive";
                    //_TrainSyllabus.RemoveCompositeKey(train.ProgramId, existingsyllabus);
                    _TrainSyllabus.Update(existingsyllabus);
                }

                // Update or add new syllabuses
                foreach (var syllabus in model.TrainingProgram_Syllabus)
                {
                    var existingSyllabus = existingsyllabuses.FirstOrDefault(s => s.SyllabusId == syllabus.SyllabusId);
                    if (existingSyllabus != null)
                    {
                        existingSyllabus.Status = "Active";
                        _TrainSyllabus.Update(existingSyllabus);
                    }
                    else
                    {
                        var newSyllabus = new TrainingProgram_Syllabus
                        {
                            TrainingProgramId = train.ProgramId,
                            SyllabusId = syllabus.SyllabusId,
                            Status = "Active"
                        };

                        await _TrainSyllabus.AddAsync(newSyllabus);
                    }
                }
                if (!string.IsNullOrEmpty(train.ProgramName))
                {
                    programExist.ProgramName = train.ProgramName;
                }

                if (!string.IsNullOrEmpty(train.Description))
                {
                    programExist.Description = train.Description;
                }

                if (train.StartDate != default(DateTime))
                {
                    programExist.StartDate = train.StartDate.Value.Date;
                }

                if (train.EndDate != default(DateTime))
                {
                    programExist.EndDate = train.EndDate.Value.Date;
                }
                if (programExist.Version != null)
                {
                    var splitVer = programExist.Version.Split(".");
                    int.TryParse(splitVer[0], out int majorVer);
                    int.TryParse(splitVer[1], out int minorVer);
                    programExist.Version = $"{majorVer}.{++minorVer}";
                }
                programExist.LastModifiedDate = train.LastModifiedDate.Value;
                programExist.LastUpdatedBy = train.LastUpdatedBy;
                var trainPro = _trainPro.Update(programExist);
                var result = await _trainPro.Commit();
                if (result > 0)
                {
                    var viewProgram = await GetTrainingProgramById(train.ProgramId);
                    return viewProgram;
                    //return _mapper.Map<TrainingProgramModel>(train);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new BadRequestException("Training program need to be actived before update!");
            }
        }
        public async Task<TrainingProgramModel> UpdateStatusTrainingProgram(int id, string status, string modifiedBy)
        {
            var search = await _trainPro.GetByIdAsync(id);
            if (search == null)
            {
                throw new NotFoundException("Training Program does not exist!");
            }
            var processReq = char.ToUpper(status[0]) + status.Substring(1).ToLower();
            search.LastModifiedDate = DateTime.Now.Date;
            search.LastUpdatedBy = modifiedBy;
            search.Status = processReq;
            _trainPro.Update(search);
            var result = await _trainPro.Commit();
            if (result > 0)
            {
                var viewTrain = await GetTrainingProgramById(search.ProgramId);
                return viewTrain;
            }
            return null;
        }


        public async Task<TrainingProgramModel> GetTrainingProgramById(int id)
        {
            var search = await _trainPro.GetByIdAsync(id);
            if (search == null)
            {
                throw new NotFoundException("Training Program does not exist!");
            }

            return _mapper.Map<TrainingProgramModel>(search);
        }
        public TrainingProgramModel? GetTrainingProgramByName(string name)
        {
            var result = _trainPro.FindByCondition(x => x.ProgramName.ToLower().Equals(name.ToLower())).FirstOrDefault();
            if (result != null)
            {
                return _mapper.Map<TrainingProgramModel>(result);
            }
            return null;
        }
        public async Task<List<TrainingProgramModel>> SearchTrainingProgram(string? keyword, int? pageNumber, int? pageSize, Func<TrainingProgram, bool> filter = null)
        {
            if (pageNumber is null || pageSize is null)
            {
                throw new BadRequestException("pageNumber and pageSize are required!");
            }
            List<string> keywords = new List<string>();
            if (!string.IsNullOrEmpty(keyword))
            {
                keywords = keyword.Split(',').Select(k => k.Trim()).ToList();
            }
            if (keyword.IsNullOrEmpty())
            {
                var getAll = _trainPro.GetAll().ToList();
                if (filter != null)
                {
                    getAll = getAll.Where(filter).ToList();
                }
                var skipForNull = (pageNumber.Value - 1) * pageSize.Value;
                var pagedSearchForNull = getAll.Skip(skipForNull).Take(pageSize.Value).ToList();
                return _mapper.Map<List<TrainingProgramModel>>(pagedSearchForNull);
            }
            //var search = _trainPro.FindByCondition(x =>x.ProgramName.ToLower().Contains(keyword.ToLower())).ToList();
            var resultList = new List<TrainingProgram>();
            foreach (var key in keywords)
            {
                var tempResult = _trainPro.FindByCondition(x => x.ProgramName.ToLower().Contains(key.ToLower())).ToList();
                resultList.AddRange(tempResult);
            }
            if (filter != null)
            {
                resultList = resultList.Where(filter).ToList();
            }
            var distinctResults = resultList.GroupBy(p => p.ProgramId)
                                .Select(g => g.First())
                                .ToList();
            var skip = (pageNumber.Value - 1) * pageSize.Value;
            var pagedSearch = distinctResults.Skip(skip).Take(pageSize.Value).ToList();
            var returnModel = _mapper.Map<List<TrainingProgramModel>>(pagedSearch);
            return returnModel;
        }

        public int GetAllSlotInTrainingProgram(int programId)
        {
            int totalSlot = 0;
            var program = _trainPro.FindByCondition(p => p.ProgramId == programId)
                .Include(p => p.TrainingProgram_Syllabus.Where(tps => tps.TrainingProgramId == programId))
                .FirstOrDefault();
            if (program is null)
            {
                throw new BadRequestException("This training program does not exist");
            }
            foreach (var tps in program.TrainingProgram_Syllabus)
            {
                var trainingUnits = _trainingUnit.FindByCondition(tu => tu.SyllabusId == tps.SyllabusId)
                    .ToList();
                if (trainingUnits is null)
                {
                    throw new BadRequestException("There is no training program unit for this program");
                }

                totalSlot += trainingUnits.Count();
            }

            return totalSlot;
        }

        public List<TrainingProgramUnit> GetAllTrainingProgramUnitByProgramId(int programId)
        {
            var program = _trainPro.FindByCondition(p => p.ProgramId == programId)
                .Include(p => p.TrainingProgram_Syllabus.Where(tps => tps.TrainingProgramId == programId))
                .FirstOrDefault();
            if (program is null)
            {
                throw new BadRequestException("This training program does not exist");
            }

            List<TrainingProgramUnit>? units = new List<TrainingProgramUnit>();
            foreach (var tps in program.TrainingProgram_Syllabus)
            {
                var trainingUnits = _trainingUnit.FindByCondition(tu => tu.SyllabusId == tps.SyllabusId)
                    .ToList();
                if (trainingUnits is null)
                {
                    throw new BadRequestException("There is no training program unit for this program");
                }

                foreach (var unit in trainingUnits)
                {
                    units.Add(unit);
                }
            }
            return units;
        }

    }
}
