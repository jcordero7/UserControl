using UserControl.Core.CustomEntities;
using UserControl.Core.Entities;
using UserControl.Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserControl.Application.Interfaces.services
{
    public interface IPostService
    {
        PagedList<Post> GetPosts(PostQueryFilter filters);

        Task<Post> GetPost(int id);

        Task InsertPost(Post post);

        Task<bool> UpdatePost(Post post);

        Task<bool> DeletePost(int id);
    }
}