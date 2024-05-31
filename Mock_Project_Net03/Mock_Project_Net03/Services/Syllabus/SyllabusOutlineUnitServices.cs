using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using System.Net.WebSockets;

namespace Mock_Project_Net03.Services.Syllabus
{
    public class SyllabusOutlineUnitServices
    {
        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepo;
        private readonly IMapper _mapper;
        private readonly IRepository<Mock_Project_Net03.Entities.Syllabus, int> _sy;

        public SyllabusOutlineUnitServices(IRepository<TrainingProgramUnit, int> trainingProgramUnitRepo, IMapper mapper, IRepository<Mock_Project_Net03.Entities.Syllabus, int> syllabusRepo)
        {
            _trainingProgramUnitRepo = trainingProgramUnitRepo;
            _mapper = mapper;
            _sy = syllabusRepo;
        }
        public async Task<TrainingProgramUnitModel> CreateSyllabusUnit(TrainingProgramUnitModel newUnit)
        {
            var unit = _mapper.Map<TrainingProgramUnitModel>(newUnit);
            if (!int.TryParse(newUnit.UnitName, out int unitNameAsNumber) || unitNameAsNumber <= 0)
            {
                throw new BadRequestException("UnitName must be a positive number");
            }
            if (newUnit.SyllabusId <= 0)
            {
                throw new BadRequestException("Invalid SyllabusId");
            }
            var syllabusIdExits = _sy.FindByCondition(y => y.SyllabusId == newUnit.SyllabusId).FirstOrDefault();
            if (syllabusIdExits == null)
            {
                throw new BadRequestException("This syllabus does not exist!");
            }
            var existingUnit = _trainingProgramUnitRepo.FindByCondition(u => u.SyllabusId == newUnit.SyllabusId && u.UnitName == newUnit.UnitName).FirstOrDefault();

            if (existingUnit != null)
            {
                throw new BadRequestException("UnitName already exists for this SyllabusId");
            }

            var trainingProgramUnitEntity = new TrainingProgramUnit
            {
                UnitName = newUnit.UnitName,
                Description = newUnit.Description,
                Time = newUnit.Time,
            };
            trainingProgramUnitEntity.SyllabusId = newUnit.SyllabusId;
            trainingProgramUnitEntity = await _trainingProgramUnitRepo.AddAsync(trainingProgramUnitEntity);
            var result = await _trainingProgramUnitRepo.Commit();
            return newUnit;
        }

    }
}
