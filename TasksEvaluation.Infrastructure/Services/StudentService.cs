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
    public class StudentService : IStudentService
    {
        private readonly IBaseMapper<Student, StudentDTO> _studentDTOMapper;
        private readonly IBaseMapper<StudentDTO, Student> _studentMapper;
        private readonly IBaseRepository<Student> _studentRepository;

        public StudentService(
            IBaseMapper<Student, StudentDTO> studentDTOMapper,
            IBaseMapper<StudentDTO, Student> studentMapper,
            IBaseRepository<Student> studentRepository)
        {
            _studentDTOMapper = studentDTOMapper;
            _studentMapper = studentMapper;
            _studentRepository = studentRepository;
        }

        public async Task<StudentDTO> Create(StudentDTO model)
        {
            var entity = _studentMapper.MapModel(model);
            entity.EntryDate = DateTime.Now;
            return _studentDTOMapper.MapModel(await _studentRepository.Create(entity));
        }

        public async Task Delete(int id)
        {
            var entity = await _studentRepository.GetById(id);
            await _studentRepository.Delete(entity);
        }

        public async Task<StudentDTO> GetStudent(int id) => _studentDTOMapper.MapModel(await _studentRepository.GetById(id));

        public async Task<IEnumerable<StudentDTO>> GetStudents() => _studentDTOMapper.MapList(await _studentRepository.GetAll());

        public async Task Update(StudentDTO model)
        {
            var existingData = _studentMapper.MapModel(model);
            existingData.UpdateDate = DateTime.Now;
            await _studentRepository.Update(existingData);
        }
    }
}
