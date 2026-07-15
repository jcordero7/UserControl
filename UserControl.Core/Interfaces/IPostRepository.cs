using UserControl.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserControl.Core.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetPostsByUser(int UserId);

     


    }
}
