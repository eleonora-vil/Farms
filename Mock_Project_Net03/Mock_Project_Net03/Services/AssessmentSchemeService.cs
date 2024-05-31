using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Repositories;

namespace Mock_Project_Net03.Services
{
    public class AssessmentSchemeService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<AssessmentScheme, int> _assessmentSchemeRepository;
        public AssessmentSchemeService(IMapper mapper, IRepository<AssessmentScheme, int> assessmentSchemeRepository)
        {
            _mapper = mapper;
            _assessmentSchemeRepository = assessmentSchemeRepository;
        }
        public async Task<AssessmentScheme_ToAdd> CreateAssessmentScheme(AssessmentScheme_ToAdd assessmentSchemeModel)
        {
            try
            {
                var assessmentScheme = _mapper.Map<AssessmentScheme>(assessmentSchemeModel);
                var existedassessmentScheme = await _assessmentSchemeRepository.FindByCondition(m => m.AssessmentSchemeName.ToLower().Equals(assessmentSchemeModel.AssessmentSchemeName.ToLower())).FirstOrDefaultAsync();
                if (existedassessmentScheme != null)
                {
                    throw new BadHttpRequestException("This AssessmentScheme has been existed");
                }
                await _assessmentSchemeRepository.AddAsync(assessmentScheme);
                int result = await _assessmentSchemeRepository.Commit();
                if (result > 0)
                {
                    return _mapper.Map<AssessmentScheme_ToAdd>(assessmentScheme);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public async Task<AssessmentScheme_ToAdd> GetAssessmentSchemeById(int id)
        {
            var assessmentScheme = await _assessmentSchemeRepository.FindByCondition(o => o.AssessmentSchemeId == id).Include(o => o.AssessmentScheme_Syllabus).FirstOrDefaultAsync();
            if (assessmentScheme == null)
            {
                throw new BadHttpRequestException("This AssessmentScheme hasn't been existed");
            }
            var asModel = _mapper.Map<AssessmentScheme_ToAdd>(assessmentScheme);
            return asModel;

        }
        public async Task<List<AssessmentScheme_ToAdd>> GetAssessmentSchemes()
        {
            var assessmentSchemes = _assessmentSchemeRepository.GetAll();
            if (assessmentSchemes == null)
            {
                throw new BadHttpRequestException("This AssessmentScheme hasn't been existed");
            }
            var asModels = _mapper.Map<List<AssessmentScheme_ToAdd>>(assessmentSchemes);
            return asModels;

        }

    }
}
