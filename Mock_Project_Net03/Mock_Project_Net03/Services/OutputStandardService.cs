using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Repositories;

namespace Mock_Project_Net03.Services
{
    public class OutputStandardService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<OutputStandard, int> _outPutStandardRepository;
        public OutputStandardService(IMapper mapper, IRepository<OutputStandard, int> outPutStandardRepository)
        {
            _mapper = mapper;
            _outPutStandardRepository = outPutStandardRepository;
        }
        public async Task<OutputStandardModel> CreateOutPutStandard(OutputStandardModel outputStandardModel)
        {
            var outPutStandard = _mapper.Map<OutputStandard>(outputStandardModel);
            var existedOutPutStandard = await _outPutStandardRepository.FindByCondition(m => m.Tags.ToLower().Equals(outputStandardModel.Tags.ToLower())).FirstOrDefaultAsync();
            if(existedOutPutStandard != null)
            {
                throw new BadHttpRequestException("This OutputStandard has been existed");
            }
            await _outPutStandardRepository.AddAsync(outPutStandard);
            int result = await _outPutStandardRepository.Commit();
            if (result > 0)
            {               
                return _mapper.Map<OutputStandardModel>(outPutStandard);
            }
            return null;
        }
        public async Task<OutputStandardModel> GetOutPutStandardById(int id)
        {
            try
            {
                var outPutStandard = await _outPutStandardRepository.GetByIdAsync(id);
                if (outPutStandard == null)
                {
                    throw new BadHttpRequestException("This OutputStandard hasn't been existed");
                }
                var opModel = _mapper.Map<OutputStandardModel>(outPutStandard);
                return opModel;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<OutputStandardModel>> GetAllOutPutStandards()
        {
            var outPutStandards = _outPutStandardRepository.GetAll();
            if (outPutStandards == null)
            {
                throw new BadHttpRequestException("This OutputStandard hasn't been existed");
            }
            var opModels = _mapper.Map<List<OutputStandardModel>>(outPutStandards);
            return opModels;

        }
    }
}
