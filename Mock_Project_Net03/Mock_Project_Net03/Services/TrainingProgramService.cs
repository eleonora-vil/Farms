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

namespace Mock_Project_Net03.Services
{
    public class TrainingProgramServices
    {
        private readonly IRepository<TrainingProgram, int> _trainPro;
        private readonly IMapper _mapper;
        private readonly IRepository<TrainingProgram_Syllabus, int> _TrainSyllabus;
        private readonly IRepository<Entities.Syllabus, int> _sy;

        public TrainingProgramServices(IRepository<TrainingProgram, int> trainPro, IMapper mapper, IRepository<TrainingProgram_Syllabus, int> trainSyllabus, IRepository<Entities.Syllabus, int> sy)
        {
            _trainPro = trainPro;
            _mapper = mapper;
            _sy = sy;
            _TrainSyllabus = trainSyllabus;
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
                    programExist.StartDate = train.StartDate;
                }

                if (train.EndDate != default(DateTime))
                {
                    programExist.EndDate = train.EndDate;
                }
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
        public async Task<TrainingProgramModel> UpdateStatusTrainingProgram(int id, string status)
        {
            var search = await _trainPro.GetByIdAsync(id);
            if (search == null)
            {
                throw new NotFoundException("Training Program does not exist!");
            }
            var processReq = char.ToUpper(status[0]) + status.Substring(1).ToLower();
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
        public TrainingProgramModel GetTrainingProgramByName(string name)
        {
            var result = _trainPro.FindByCondition(x => x.ProgramName.ToLower().Equals(name.ToLower())).FirstOrDefault();
            if (result != null)
            {
                return _mapper.Map<TrainingProgramModel>(result);
            }
            return null;
        }
        public async Task<List<TrainingProgramModel>> SearchTrainingProgram(string? keyword, int pageNumber, int pageSize, Func<TrainingProgram, bool> filter = null)
        {
            int? isId = null;
            if (int.TryParse(keyword, out int resultParse))
            {
                isId = resultParse;
            }
            var search = _trainPro.FindByCondition(x => (isId.HasValue && x.ProgramId == isId.Value) ||
                                                        (!isId.HasValue && x.ProgramName.ToLower().Contains(keyword.ToLower()))).ToList();
            if (filter != null)
            {
                search = search.Where(filter).ToList();
            }
            var skip = (pageNumber - 1) * pageSize;
            var pagedSearch = search.Skip(skip).Take(pageSize).ToList();
            var returnModel = _mapper.Map<List<TrainingProgramModel>>(pagedSearch);
            return returnModel;
        }
    }
}
