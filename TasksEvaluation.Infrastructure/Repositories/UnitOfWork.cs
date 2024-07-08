using TasksEvaluation.Areas.Identity.Data;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.IRepositories;
using TasksEvaluation.Infrastructure.Data;

namespace TasksEvaluation.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IBaseRepositories<ApplicationUser> ApplicationUsers => new BaseRepositories<ApplicationUser>(_context);
        public IBaseRepositories<Student> Students => new BaseRepositories<Student>(_context);


        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}
