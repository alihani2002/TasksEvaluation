using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.Interfaces.IServices;
using TasksEvaluation.Core.Mapper;

namespace TasksEvaluation.Infrastructure.Services
{
    public class SolutionService : ISolutionService
    {
        private readonly IBaseMapper<Solution, SolutionDTO> _solutionDTOMapper;
        private readonly IBaseMapper<SolutionDTO, Solution> _solutionMapper;
        private readonly IBaseRepository<Solution> _solutionRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private List<string> _allowedFileExtensions = new() { ".pdf", ".docx" };
        private int _maxAllowedSizeFile = 5242880;

        public SolutionService(
            IBaseMapper<Solution, SolutionDTO> solutionDTOMapper,
            IBaseMapper<SolutionDTO, Solution> solutionMapper,
            IBaseRepository<Solution> solutionRepository,
          IWebHostEnvironment webHostEnvironment)
        {
            _solutionDTOMapper = solutionDTOMapper;
            _solutionMapper = solutionMapper;
            _solutionRepository = solutionRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<SolutionDTO> Create(SolutionDTO model)
        {
            var entity = _solutionMapper.MapModel(model);
            entity.EntryDate = DateTime.Now;
            return _solutionDTOMapper.MapModel(await _solutionRepository.Create(entity));
        }

        public async Task Delete(int id)
        {
            var entity = await _solutionRepository.GetById(id);
            await _solutionRepository.Delete(entity);
        }

        public async Task<SolutionDTO> GetSolution(int id) => _solutionDTOMapper.MapModel(await _solutionRepository.GetById(id));

        public async Task<IEnumerable<SolutionDTO>> GetSolutions() => _solutionDTOMapper.MapList(await _solutionRepository.GetAll());

        public async Task Update(SolutionDTO model)
        {
            var existingData = _solutionMapper.MapModel(model);
            existingData.UpdateDate = DateTime.Now;
            await _solutionRepository.Update(existingData);
        }

        public async Task<SolutionDTO> UploadSolution(UploadSolutionDTO model)
        {
            var solution = _solutionDTOMapper.MapModel(await _solutionRepository.GetById(model.Id));
            var extension = Path.GetExtension(model.SolutionFile.FileName);

            if (!_allowedFileExtensions.Contains(extension))
                return new SolutionDTO { Notes = "Only .pdf, .docx files are allowed!" };

            if (model.SolutionFile.Length > _maxAllowedSizeFile)
                return new SolutionDTO { Notes = "File cannot be more than 5 MB!" };

            var fileName = $"{Guid.NewGuid()}{extension}";

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/pdfs", fileName);
            using var stream = File.Create(path);
            model.SolutionFile.CopyTo(stream);

            solution.SolutionFile = fileName;
            return  solution;
        }
    }
}
