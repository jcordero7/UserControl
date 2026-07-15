using UserControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserControl.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        IUserRepository UserRepository { get; }

        IRepository<Commentary> CommentaryRepository { get; }

        ISecurityRepository SecurityRepository { get; }

        IProgramXUserRepository ProgramXUserRepository { get; }

        void SaveChanges();

        Task SaveChangesAsyn();

    }
}

