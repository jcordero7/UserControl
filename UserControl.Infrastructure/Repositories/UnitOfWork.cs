using UserControl.Core.Entities;
using UserControl.Core.Interfaces;
using UserControl.Infrastructure.Data;
using System.Threading.Tasks;

namespace UserControl.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserControlContext _context;
        // private readonly DbSet<T> _entities;

        private readonly IPostRepository _postRepository;

        //private readonly IRepository<User> _userRepository;
        private readonly IUserRepository _userRepository;

        private readonly IRepository<Commentary> _commentaryRepository;
        private readonly ISecurityRepository _securityRepository;

        private readonly IProgramXUserRepository _programXUserRepository;

        public UnitOfWork(UserControlContext context)
        {
            _context = context;
        }

        public IPostRepository PostRepository => _postRepository ?? new PostRepository(_context);

        //public IRepository<User> UserRepository => _userRepository ?? new BaseRepository<User>(_context);

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

        public IRepository<Commentary> CommentaryRepository => _commentaryRepository ?? new BaseRepository<Commentary>(_context);

        public ISecurityRepository SecurityRepository => _securityRepository ?? new SecurityRepository(_context);

        public IProgramXUserRepository ProgramXUserRepository => _programXUserRepository ?? new ProgramXUserRepository(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsyn()
        {
           await _context.SaveChangesAsync();
        }

    }
}
