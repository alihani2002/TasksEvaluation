using System;
using System.Collections.Generic;
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
            if (model == null) throw new ArgumentNullException(nameof(model));

            var entity = _studentMapper.MapModel(model);
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            entity.EntryDate = DateTime.Now;
            var createdEntity = await _studentRepository.Create(entity);
            return _studentDTOMapper.MapModel(createdEntity);
        }

        public async Task Delete(int id)
        {
            var entity = await _studentRepository.GetById(id);
            if (entity == null) throw new ArgumentNullException($"Student with ID {id} not found");

            await _studentRepository.Delete(entity);
        }

        public async Task<StudentDTO> GetStudent(int id)
        {
            var entity = await _studentRepository.GetById(id);
            if (entity == null) throw new ArgumentNullException($"Student with ID {id} not found");

            return _studentDTOMapper.MapModel(entity);
        }

        public async Task<IEnumerable<StudentDTO>> GetStudents()
        {
            var entities = await _studentRepository.GetAll();
            return _studentDTOMapper.MapList(entities);
        }

        public async Task Update(StudentDTO model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var student = await _studentRepository.GetById(model.Id);
            if (student == null) throw new ArgumentNullException($"Student with ID {model.Id} not found");

            student = _studentMapper.MapModel(model);
            student.UpdateDate = DateTime.Now;
            await _studentRepository.Update(student);
        }
    }
}
